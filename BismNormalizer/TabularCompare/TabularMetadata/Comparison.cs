using System;
using System.Collections.Generic;
using BismNormalizer.TabularCompare.Core;
using Microsoft.AnalysisServices.Tabular;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Represents a source vs. target comparison of an SSAS tabular model. This class is for tabular models that use tabular metadata with SSAS compatibility level 1200 or above.
    /// </summary>
    public class Comparison : Core.Comparison
    {
        #region Private Variables

        private TabularModel _sourceTabularModel;
        private TabularModel _targetTabularModel;
        private bool _uncommitedChanges = false;
        private DateTime _lastSourceSchemaUpdate = DateTime.MinValue;
        private DateTime _lastTargetSchemaUpdate = DateTime.MinValue;
        private bool _disposed = false;

        #endregion

        #region Properties

        /// <summary>
        /// TabularModel object being used as the source for comparison.
        /// </summary>
        public TabularModel SourceTabularModel
        {
            get { return _sourceTabularModel; }
            set { _sourceTabularModel = value; }
        }

        /// <summary>
        /// TabularModel object being used as the target for comparison.
        /// </summary>
        public TabularModel TargetTabularModel
        {
            get { return _targetTabularModel; }
            set { _targetTabularModel = value; }
        }

        #endregion

        public Comparison(ComparisonInfo comparisonInfo)
            : base(comparisonInfo)
        {
            _sourceTabularModel = new TabularModel(this, comparisonInfo.ConnectionInfoSource, comparisonInfo);
            _targetTabularModel = new TabularModel(this, comparisonInfo.ConnectionInfoTarget, comparisonInfo);
        }

        /// <summary>
        /// Connect to source and target tabular models, and instantiate their properties.
        /// </summary>
        public override void Connect()
        {
            _sourceTabularModel.Connect();
            _targetTabularModel.Connect();
        }

        /// <summary>
        /// Disconnect from source and target tabular models.
        /// </summary>
        public override void Disconnect()
        {
            _sourceTabularModel.Disconnect();
            _targetTabularModel.Disconnect();
        }

        public override void CompareTabularModels()
        {
            _comparisonObjectCount = 0;

            #region Connections

            foreach (Connection connectionSource in _sourceTabularModel.Connections)
            {
                // check if source is not in target
                if (!_targetTabularModel.Connections.ContainsName(connectionSource.Name))
                {
                    ComparisonObject comparisonObjectConnection = new ComparisonObject(ComparisonObjectType.Connection, ComparisonObjectStatus.MissingInTarget, connectionSource, null, MergeAction.Create);
                    _comparisonObjects.Add(comparisonObjectConnection);
                    _comparisonObjectCount += 1;

                    #region Tables for Connection that is Missing in Target (to be created in target)

                    foreach (Table tblSource in _sourceTabularModel.Tables.FilterByConnection(connectionSource.TomConnection))
                    {
                        ComparisonObject comparisonObjectTable = new ComparisonObject(ComparisonObjectType.Table, ComparisonObjectStatus.MissingInTarget, tblSource, null, MergeAction.Create);
                        comparisonObjectConnection.ChildComparisonObjects.Add(comparisonObjectTable);
                        _comparisonObjectCount += 1;

                        #region Relationships for Table that is Missing in Target

                        foreach (Relationship relSource in tblSource.Relationships)
                        {
                            ComparisonObject comparisonObjectRelation = new ComparisonObject(ComparisonObjectType.Relationship, ComparisonObjectStatus.MissingInTarget, relSource, null, MergeAction.Create);
                            comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectRelation);
                            _comparisonObjectCount += 1;
                        }

                        #endregion

                        #region Measures / KPIs for Table that is Missing in Target

                        foreach (Measure measureSource in tblSource.Measures.FilterByTableName(tblSource.Name))
                        {
                            ComparisonObjectType comparisonObjectType = measureSource.IsKpi ? ComparisonObjectType.Kpi : ComparisonObjectType.Measure;
                            ComparisonObject comparisonObjectMeasure = new ComparisonObject(comparisonObjectType, ComparisonObjectStatus.MissingInTarget, measureSource, null, MergeAction.Create);
                            comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectMeasure);
                            _comparisonObjectCount += 1;
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    // there is a connection in the target with the same name at least
                    Connection connectionTarget = _targetTabularModel.Connections.FindByName(connectionSource.Name);
                    ComparisonObject comparisonObjectConnection;
                    
                    // check if connection object definition is different
                    if (connectionSource.ObjectDefinition != connectionTarget.ObjectDefinition)
                    {
                        comparisonObjectConnection = new ComparisonObject(ComparisonObjectType.Connection, ComparisonObjectStatus.DifferentDefinitions, connectionSource, connectionTarget, MergeAction.Update);
                        _comparisonObjects.Add(comparisonObjectConnection);
                        _comparisonObjectCount += 1;
                    }
                    else
                    {
                        // they are equal, ...
                        comparisonObjectConnection = new ComparisonObject(ComparisonObjectType.Connection, ComparisonObjectStatus.SameDefinition, connectionSource, connectionTarget, MergeAction.Skip);
                        _comparisonObjects.Add(comparisonObjectConnection);
                        _comparisonObjectCount += 1;
                    }

                    #region Tables where source/target connections exist

                    foreach (Table tblSource in _sourceTabularModel.Tables.FilterByConnection(connectionSource.TomConnection))
                    {
                        // check if source is not in target
                        TableCollection targetTablesForComparison = _targetTabularModel.Tables.FilterByConnection(connectionTarget.TomConnection);

                        CompareSourceTableToTargetTables(comparisonObjectConnection.ChildComparisonObjects, tblSource, targetTablesForComparison);
                    }

                    foreach (Table tblTarget in _targetTabularModel.Tables.FilterByConnection(connectionTarget.TomConnection))
                    {
                        // check if target is not in source
                        if (!_sourceTabularModel.Tables.FilterByConnection(connectionSource.TomConnection).ContainsName(tblTarget.Name))
                        {
                            CaptureTargetTableMissingInSource(comparisonObjectConnection.ChildComparisonObjects, tblTarget);
                        }
                    }
                    #endregion
                }
            }

            foreach (Connection connectionTarget in _targetTabularModel.Connections)
            {
                // if target connection is Missing in Source, offer deletion
                if (!_sourceTabularModel.Connections.ContainsName(connectionTarget.Name))
                {
                    ComparisonObject comparisonObjectConnection = new ComparisonObject(ComparisonObjectType.Connection, ComparisonObjectStatus.MissingInSource, null, connectionTarget, MergeAction.Delete);
                    _comparisonObjects.Add(comparisonObjectConnection);
                    _comparisonObjectCount += 1;

                    #region Tables for Connection that is Missing in Source

                    foreach (Table tableTarget in _targetTabularModel.Tables.FilterByConnection(connectionTarget.TomConnection))
                    {
                        CaptureTargetTableMissingInSource(comparisonObjectConnection.ChildComparisonObjects, tableTarget);
                    }

                    #endregion
                }
            }

            #endregion

            #region Calculated tables (don't have a connection)

            foreach (Table tblSource in _sourceTabularModel.Tables.WithoutConnection(_sourceTabularModel.TomDatabase.Model))
            {
                // check if source is not in target
                TableCollection targetTablesForComparison = _targetTabularModel.Tables.WithoutConnection(_targetTabularModel.TomDatabase.Model);

                CompareSourceTableToTargetTables(_comparisonObjects, tblSource, targetTablesForComparison);
            }

            foreach (Table tblTarget in _targetTabularModel.Tables.WithoutConnection(_targetTabularModel.TomDatabase.Model))
            {
                // check if target is not in source
                if (!_sourceTabularModel.Tables.WithoutConnection(_sourceTabularModel.TomDatabase.Model).ContainsName(tblTarget.Name))
                {
                    CaptureTargetTableMissingInSource(_comparisonObjects, tblTarget);
                }
            }
            
            #endregion

            #region Expressions

            foreach (Expression expressionSource in _sourceTabularModel.Expressions)
            {
                // check if source is not in target
                if (!_targetTabularModel.Expressions.ContainsName(expressionSource.Name))
                {
                    ComparisonObject comparisonObjectExpression = new ComparisonObject(ComparisonObjectType.Expression, ComparisonObjectStatus.MissingInTarget, expressionSource, null, MergeAction.Create);
                    _comparisonObjects.Add(comparisonObjectExpression);
                    _comparisonObjectCount += 1;
                }
                else
                {
                    // there is a expression in the target with the same name at least
                    Expression expressionTarget = _targetTabularModel.Expressions.FindByName(expressionSource.Name);
                    ComparisonObject comparisonObjectExpression;

                    // check if expression object definition is different
                    if (expressionSource.ObjectDefinition != expressionTarget.ObjectDefinition)
                    {
                        comparisonObjectExpression = new ComparisonObject(ComparisonObjectType.Expression, ComparisonObjectStatus.DifferentDefinitions, expressionSource, expressionTarget, MergeAction.Update);
                        _comparisonObjects.Add(comparisonObjectExpression);
                        _comparisonObjectCount += 1;
                    }
                    else
                    {
                        // they are equal, ...
                        comparisonObjectExpression = new ComparisonObject(ComparisonObjectType.Expression, ComparisonObjectStatus.SameDefinition, expressionSource, expressionTarget, MergeAction.Skip);
                        _comparisonObjects.Add(comparisonObjectExpression);
                        _comparisonObjectCount += 1;
                    }
                }
            }

            foreach (Expression expressionTarget in _targetTabularModel.Expressions)
            {
                // if target expression is Missing in Source, offer deletion
                if (!_sourceTabularModel.Expressions.ContainsName(expressionTarget.Name))
                {
                    ComparisonObject comparisonObjectExpression = new ComparisonObject(ComparisonObjectType.Expression, ComparisonObjectStatus.MissingInSource, null, expressionTarget, MergeAction.Delete);
                    _comparisonObjects.Add(comparisonObjectExpression);
                    _comparisonObjectCount += 1;
                }
            }

            #endregion

            #region Perspectives

            if (_comparisonInfo.OptionsInfo.OptionPerspectives)
            {
                foreach (Perspective perspectiveSource in _sourceTabularModel.Perspectives)
                {
                    // check if source is not in target
                    if (!_targetTabularModel.Perspectives.ContainsName(perspectiveSource.Name))
                    {
                        ComparisonObject comparisonObjectPerspective = new ComparisonObject(ComparisonObjectType.Perspective, ComparisonObjectStatus.MissingInTarget, perspectiveSource, null, MergeAction.Create);
                        _comparisonObjects.Add(comparisonObjectPerspective);
                        _comparisonObjectCount += 1;
                    }
                    else
                    {
                        // there is a perspective in the target with the same name at least
                        Perspective perspectiveTarget = _targetTabularModel.Perspectives.FindByName(perspectiveSource.Name);
                        ComparisonObject comparisonObjectPerspective;

                        // check if perspective object definition is different
                        //if (perspectiveSource.ObjectDefinition != perspectiveTarget.ObjectDefinition)
                        if ( (_comparisonInfo.OptionsInfo.OptionMergePerspectives && perspectiveTarget.ContainsOtherPerspectiveSelections(perspectiveSource)) ||
                             (!_comparisonInfo.OptionsInfo.OptionMergePerspectives && perspectiveTarget.ContainsOtherPerspectiveSelections(perspectiveSource) && perspectiveSource.ContainsOtherPerspectiveSelections(perspectiveTarget)) )
                        {
                            // they are equal, ...
                            comparisonObjectPerspective = new ComparisonObject(ComparisonObjectType.Perspective, ComparisonObjectStatus.SameDefinition, perspectiveSource, perspectiveTarget, MergeAction.Skip);
                            _comparisonObjects.Add(comparisonObjectPerspective);
                            _comparisonObjectCount += 1;
                        }
                        else
                        {
                            comparisonObjectPerspective = new ComparisonObject(ComparisonObjectType.Perspective, ComparisonObjectStatus.DifferentDefinitions, perspectiveSource, perspectiveTarget, MergeAction.Update);
                            _comparisonObjects.Add(comparisonObjectPerspective);
                            _comparisonObjectCount += 1;
                        }
                    }
                }

                foreach (Perspective perspectiveTarget in _targetTabularModel.Perspectives)
                {
                    // if target perspective is Missing in Source, offer deletion
                    if (!_sourceTabularModel.Perspectives.ContainsName(perspectiveTarget.Name))
                    {
                        ComparisonObject comparisonObjectPerspective = new ComparisonObject(ComparisonObjectType.Perspective, ComparisonObjectStatus.MissingInSource, null, perspectiveTarget, MergeAction.Delete);
                        _comparisonObjects.Add(comparisonObjectPerspective);
                        _comparisonObjectCount += 1;
                    }
                }
            }

            #endregion

            #region Cultures

            if (_comparisonInfo.OptionsInfo.OptionCultures)
            {
                foreach (Culture cultureSource in _sourceTabularModel.Cultures)
                {
                    // check if source is not in target
                    if (!_targetTabularModel.Cultures.ContainsName(cultureSource.Name))
                    {
                        ComparisonObject comparisonObjectCulture = new ComparisonObject(ComparisonObjectType.Culture, ComparisonObjectStatus.MissingInTarget, cultureSource, null, MergeAction.Create);
                        _comparisonObjects.Add(comparisonObjectCulture);
                        _comparisonObjectCount += 1;
                    }
                    else
                    {
                        // there is a culture in the target with the same name at least
                        Culture cultureTarget = _targetTabularModel.Cultures.FindByName(cultureSource.Name);
                        ComparisonObject comparisonObjectCulture;

                        // check if culture object definition is different
                        //if (cultureSource.ObjectDefinition != cultureTarget.ObjectDefinition)
                        if ((_comparisonInfo.OptionsInfo.OptionMergeCultures && cultureTarget.ContainsOtherCultureTranslations(cultureSource)) ||
                             (!_comparisonInfo.OptionsInfo.OptionMergeCultures && cultureTarget.ContainsOtherCultureTranslations(cultureSource) && cultureSource.ContainsOtherCultureTranslations(cultureTarget)))
                        {
                            // they are equal, ...
                            comparisonObjectCulture = new ComparisonObject(ComparisonObjectType.Culture, ComparisonObjectStatus.SameDefinition, cultureSource, cultureTarget, MergeAction.Skip);
                            _comparisonObjects.Add(comparisonObjectCulture);
                            _comparisonObjectCount += 1;
                        }
                        else
                        {
                            comparisonObjectCulture = new ComparisonObject(ComparisonObjectType.Culture, ComparisonObjectStatus.DifferentDefinitions, cultureSource, cultureTarget, MergeAction.Update);
                            _comparisonObjects.Add(comparisonObjectCulture);
                            _comparisonObjectCount += 1;
                        }
                    }
                }

                foreach (Culture cultureTarget in _targetTabularModel.Cultures)
                {
                    // if target culture is Missing in Source, offer deletion
                    if (!_sourceTabularModel.Cultures.ContainsName(cultureTarget.Name))
                    {
                        ComparisonObject comparisonObjectCulture = new ComparisonObject(ComparisonObjectType.Culture, ComparisonObjectStatus.MissingInSource, null, cultureTarget, MergeAction.Delete);
                        _comparisonObjects.Add(comparisonObjectCulture);
                        _comparisonObjectCount += 1;
                    }
                }
            }

            #endregion

            #region Roles

            if (_comparisonInfo.OptionsInfo.OptionRoles)
            {
                foreach (Role roleSource in _sourceTabularModel.Roles)
                {
                    // check if source is not in target
                    if (!_targetTabularModel.Roles.ContainsName(roleSource.Name))
                    {
                        ComparisonObject comparisonObjectRole = new ComparisonObject(ComparisonObjectType.Role, ComparisonObjectStatus.MissingInTarget, roleSource, null, MergeAction.Create);
                        _comparisonObjects.Add(comparisonObjectRole);
                        _comparisonObjectCount += 1;
                    }
                    else
                    {
                        // there is a role in the target with the same name at least
                        Role roleTarget = _targetTabularModel.Roles.FindByName(roleSource.Name);
                        ComparisonObject comparisonObjectRole;

                        // check if role object definition is different
                        if (roleSource.ObjectDefinition != roleTarget.ObjectDefinition)
                        {
                            comparisonObjectRole = new ComparisonObject(ComparisonObjectType.Role, ComparisonObjectStatus.DifferentDefinitions, roleSource, roleTarget, MergeAction.Update);
                            _comparisonObjects.Add(comparisonObjectRole);
                            _comparisonObjectCount += 1;
                        }
                        else
                        {
                            // they are equal, ...
                            comparisonObjectRole = new ComparisonObject(ComparisonObjectType.Role, ComparisonObjectStatus.SameDefinition, roleSource, roleTarget, MergeAction.Skip);
                            _comparisonObjects.Add(comparisonObjectRole);
                            _comparisonObjectCount += 1;
                        }
                    }
                }

                foreach (Role roleTarget in _targetTabularModel.Roles)
                {
                    // if target role is Missing in Source, offer deletion
                    if (!_sourceTabularModel.Roles.ContainsName(roleTarget.Name))
                    {
                        ComparisonObject comparisonObjectRole = new ComparisonObject(ComparisonObjectType.Role, ComparisonObjectStatus.MissingInSource, null, roleTarget, MergeAction.Delete);
                        _comparisonObjects.Add(comparisonObjectRole);
                        _comparisonObjectCount += 1;
                    }
                }
            }

            #endregion

            #region Sorting

            _comparisonObjects.Sort();
            foreach (ComparisonObject childComparisonObject in _comparisonObjects)
            {
                childComparisonObject.ChildComparisonObjects.Sort();
                foreach (ComparisonObject grandChildComparisonObject in childComparisonObject.ChildComparisonObjects)
                {
                    grandChildComparisonObject.ChildComparisonObjects.Sort();
                }
            }

            #endregion

            this.RefreshComparisonObjectsFromSkipSelections();

            _uncommitedChanges = false;
            _lastSourceSchemaUpdate = _sourceTabularModel.TomDatabase.LastSchemaUpdate;
            _lastTargetSchemaUpdate = _targetTabularModel.TomDatabase.LastSchemaUpdate;
        }

        #region Private methods for comparison

        private void CompareSourceTableToTargetTables(List<Core.ComparisonObject> comparisonObjects, Table tblSource, TableCollection targetTables)
        {
            if (!targetTables.ContainsName(tblSource.Name))
            {
                ComparisonObject comparisonObjectTable = new ComparisonObject(ComparisonObjectType.Table, ComparisonObjectStatus.MissingInTarget, tblSource, null, MergeAction.Create);
                comparisonObjects.Add(comparisonObjectTable);
                _comparisonObjectCount += 1;

                #region Relationships for table Missing in Target

                // all relationships in source are not in target (the target table doesn't even exist)
                foreach (Relationship relSource in tblSource.Relationships)
                {
                    ComparisonObject comparisonObjectRelation = new ComparisonObject(ComparisonObjectType.Relationship, ComparisonObjectStatus.MissingInTarget, relSource, null, MergeAction.Create);
                    comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectRelation);
                    _comparisonObjectCount += 1;
                }

                #endregion

                #region Measures / KPIs for Table that is Missing in Target

                foreach (Measure measureSource in tblSource.Measures.FilterByTableName(tblSource.Name))
                {
                    ComparisonObjectType comparisonObjectType = measureSource.IsKpi ? ComparisonObjectType.Kpi : ComparisonObjectType.Measure;
                    ComparisonObject comparisonObjectMeasure = new ComparisonObject(comparisonObjectType, ComparisonObjectStatus.MissingInTarget, measureSource, null, MergeAction.Create);
                    comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectMeasure);
                    _comparisonObjectCount += 1;
                }

                #endregion
            }
            else
            {
                //table name is in source and target

                Table tblTarget = _targetTabularModel.Tables.FindByName(tblSource.Name);
                ComparisonObject comparisonObjectTable;

                if (tblSource.ObjectDefinition == tblTarget.ObjectDefinition)
                {
                    comparisonObjectTable = new ComparisonObject(ComparisonObjectType.Table, ComparisonObjectStatus.SameDefinition, tblSource, tblTarget, MergeAction.Skip);
                    comparisonObjects.Add(comparisonObjectTable);
                    _comparisonObjectCount += 1;
                }
                else
                {
                    comparisonObjectTable = new ComparisonObject(ComparisonObjectType.Table, ComparisonObjectStatus.DifferentDefinitions, tblSource, tblTarget, MergeAction.Update);
                    comparisonObjects.Add(comparisonObjectTable);
                    _comparisonObjectCount += 1;
                }

                #region Relationships source/target tables exist

                foreach (Relationship relSource in tblSource.Relationships)
                {
                    // check if source is not in target
                    if (!tblTarget.Relationships.ContainsName(relSource.Name)) //Using Name, not InternalName in case internal name is different
                    {
                        ComparisonObject comparisonObjectRelation = new ComparisonObject(ComparisonObjectType.Relationship, ComparisonObjectStatus.MissingInTarget, relSource, null, MergeAction.Create);
                        comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectRelation);
                        _comparisonObjectCount += 1;
                    }
                    else
                    {
                        //relationship is in source and target

                        Relationship relTarget = tblTarget.Relationships.FindByName(relSource.Name);
                        ComparisonObject comparisonObjectRelationship;

                        if (relSource.ObjectDefinition == relTarget.ObjectDefinition)
                        {
                            comparisonObjectRelationship = new ComparisonObject(ComparisonObjectType.Relationship, ComparisonObjectStatus.SameDefinition, relSource, relTarget, MergeAction.Skip);
                            comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectRelationship);
                            _comparisonObjectCount += 1;
                        }
                        else
                        {
                            comparisonObjectRelationship = new ComparisonObject(ComparisonObjectType.Relationship, ComparisonObjectStatus.DifferentDefinitions, relSource, relTarget, MergeAction.Update);
                            comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectRelationship);
                            _comparisonObjectCount += 1;
                        }
                    }
                }

                // see if relationships in target table that don't exist in source table
                foreach (Relationship relTarget in tblTarget.Relationships)
                {
                    // check if source is not in target
                    if (!tblSource.Relationships.ContainsName(relTarget.Name)) //Using Name, not InternalName in case internal name is different
                    {
                        ComparisonObject comparisonObjectRelation = new ComparisonObject(ComparisonObjectType.Relationship, ComparisonObjectStatus.MissingInSource, null, relTarget, MergeAction.Delete);
                        comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectRelation);
                        _comparisonObjectCount += 1;
                    }
                }

                #endregion

                #region Measures / KPIs (table in source and target)

                // see if matching measure in source and target
                foreach (Measure measureSource in tblSource.Measures.FilterByTableName(tblSource.Name))
                {
                    ComparisonObjectType comparisonObjectType = measureSource.IsKpi ? ComparisonObjectType.Kpi : ComparisonObjectType.Measure;

                    if (tblTarget.Measures.FilterByTableName(tblTarget.Name).ContainsName(measureSource.Name))
                    {
                        //Measure in source and target, so check definition
                        Measure measureTarget = tblTarget.Measures.FilterByTableName(tblTarget.Name).FindByName(measureSource.Name);
                        if (measureSource.ObjectDefinition == measureTarget.ObjectDefinition)
                        {
                            //Measure has same definition
                            ComparisonObject comparisonObjectMeasure = new ComparisonObject(comparisonObjectType, ComparisonObjectStatus.SameDefinition, measureSource, measureTarget, MergeAction.Skip);
                            comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectMeasure);
                            _comparisonObjectCount += 1;
                        }
                        else
                        {
                            //Measure has different definition
                            ComparisonObject comparisonObjectMeasure = new ComparisonObject(comparisonObjectType, ComparisonObjectStatus.DifferentDefinitions, measureSource, measureTarget, MergeAction.Update);
                            comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectMeasure);
                            _comparisonObjectCount += 1;
                        }
                    }
                    else
                    {
                        ComparisonObject comparisonObjectMeasure = new ComparisonObject(comparisonObjectType, ComparisonObjectStatus.MissingInTarget, measureSource, null, MergeAction.Create);
                        comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectMeasure);
                        _comparisonObjectCount += 1;
                    }
                }
                //now check if target contains measures Missing in Source
                foreach (Measure measureTarget in tblTarget.Measures.FilterByTableName(tblTarget.Name))
                {
                    ComparisonObjectType comparisonObjectType = measureTarget.IsKpi ? ComparisonObjectType.Kpi : ComparisonObjectType.Measure;
                    if (!tblSource.Measures.FilterByTableName(tblSource.Name).ContainsName(measureTarget.Name))
                    {
                        ComparisonObject comparisonObjectMeasure = new ComparisonObject(comparisonObjectType, ComparisonObjectStatus.MissingInSource, null, measureTarget, MergeAction.Delete);
                        comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectMeasure);
                        _comparisonObjectCount += 1;
                    }
                }

                #endregion
            }
        }

        private void CaptureTargetTableMissingInSource(List<Core.ComparisonObject> comparisonObjects, Table tblTarget)
        {
            ComparisonObject comparisonObjectTable = new ComparisonObject(ComparisonObjectType.Table, ComparisonObjectStatus.MissingInSource, null, tblTarget, MergeAction.Delete);
            comparisonObjects.Add(comparisonObjectTable);
            _comparisonObjectCount += 1;

            #region Relationships for table Missing in Source

            // all relationships in target are not in source (the source table doesn't even exist)
            foreach (Relationship relTarget in tblTarget.Relationships)
            {
                ComparisonObject comparisonObjectRelation = new ComparisonObject(ComparisonObjectType.Relationship, ComparisonObjectStatus.MissingInSource, null, relTarget, MergeAction.Delete);
                comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectRelation);
                _comparisonObjectCount += 1;
            }

            #endregion

            #region Measures for Table that is Missing in Source

            foreach (Measure measureTarget in tblTarget.Measures.FilterByTableName(tblTarget.Name))
            {
                ComparisonObjectType comparisonObjectType = measureTarget.IsKpi ? ComparisonObjectType.Kpi : ComparisonObjectType.Measure;
                ComparisonObject comparisonObjectMeasure = new ComparisonObject(ComparisonObjectType.Measure, ComparisonObjectStatus.MissingInSource, null, measureTarget, MergeAction.Delete);
                comparisonObjectTable.ChildComparisonObjects.Add(comparisonObjectMeasure);
                _comparisonObjectCount += 1;
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Validate selection of actions to perform on target tabular model. Warnings and informational messages are provided by invoking ShowStatusMessageCallBack.
        /// </summary>
        public override void ValidateSelection()
        {
            #region Refresh/reconnect source and target dbs to check if server definition has changed

            if (_uncommitedChanges)
            {
                // Reconnect to re-initialize
                _sourceTabularModel = new TabularModel(this, _comparisonInfo.ConnectionInfoSource, _comparisonInfo);
                _sourceTabularModel.Connect();
                _targetTabularModel = new TabularModel(this, _comparisonInfo.ConnectionInfoTarget, _comparisonInfo);
                _targetTabularModel.Connect();
            }
            else
            {
                _sourceTabularModel.TomDatabase.Refresh();
                _targetTabularModel.TomDatabase.Refresh();
            }

            if (!_sourceTabularModel.ConnectionInfo.UseProject && _sourceTabularModel.TomDatabase.LastSchemaUpdate > _lastSourceSchemaUpdate)
            {
                throw new Exception("The definition of the source database has changed since the comparison was run.  Please re-run the comparison.");
            }
            if (!_targetTabularModel.ConnectionInfo.UseProject && _targetTabularModel.TomDatabase.LastSchemaUpdate > _lastTargetSchemaUpdate)
            {
                throw new Exception("The definition of the target database has changed since the comparison was run.  Please re-run the comparison.");
            }

            _uncommitedChanges = true;

            #endregion

            #region Iterate of objects for delete/create/updates

            #region Backup perspectives, cultures and roles

            /*It's easier to take a backup of perspectives, cultures and roles now and add back after table changes, rather than every
              time update a table, take a temp backup to add back columns/measures. Also would need to remove deleted tables/meausures, ... 
              Gets pretty hairy.
            */

            _targetTabularModel.BackupAffectedObjects();

            #endregion

            #region Connections

            // do deletions first to minimize chance of conflict
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                DeleteConnections(comparisonObject);
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                CreateConnections(comparisonObject);
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                UpdateConnections(comparisonObject);
            }

            #endregion

            #region Expressions

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                DeleteExpressions(comparisonObject);
            }

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                CreateExpressions(comparisonObject);
            }

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                UpdateExpressions(comparisonObject);
            }

            #endregion

            #region Tables

            // do deletions first to minimize chance of conflict
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                DeleteTables(comparisonObject);
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                CreateTables(comparisonObject);
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                UpdateTables(comparisonObject);
            }

            #endregion

            #region Relationships

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                DeleteRelationships(comparisonObject);
            }

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                CreateRelationships(comparisonObject, "");
            }

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                UpdateRelationships(comparisonObject, "");
            }

            _targetTabularModel.ValidateRelationships();

            #endregion

            #region Measures / KPIs

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                DeleteMeasures(comparisonObject);
            }

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                CreateMeasures(comparisonObject, "");
            }

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                UpdateMeasures(comparisonObject, "");
            }

            #endregion

            #region Perspectives

            //Restore perspectives that were backed up earlier. Having done this there won't be any dependency issues, so can start comparison changes.
            _targetTabularModel.RestorePerspectives();

            // Do separate loops of _comparisonObjectects for Delete, Create, Update to ensure informational logging order is consistent with other object types. This also ensures deletions are done first to minimize chance of conflict.
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Perspective && comparisonObject.MergeAction == MergeAction.Delete)
                {
                    _targetTabularModel.DeletePerspective(comparisonObject.TargetObjectInternalName);
                    OnValidationMessage(new ValidationMessageEventArgs($"Delete Perspective [{comparisonObject.TargetObjectName}].", ValidationMessageType.Perspective, ValidationMessageStatus.Informational));
                }
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Perspective && comparisonObject.MergeAction == MergeAction.Create)
                {
                    _targetTabularModel.CreatePerspective(_sourceTabularModel.Perspectives.FindById(comparisonObject.SourceObjectInternalName).TomPerspective);
                    OnValidationMessage(new ValidationMessageEventArgs($"Create Perspective [{comparisonObject.SourceObjectName}].", ValidationMessageType.Perspective, ValidationMessageStatus.Informational));
                }
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Perspective && comparisonObject.MergeAction == MergeAction.Update)
                {
                    _targetTabularModel.UpdatePerspective(_sourceTabularModel.Perspectives.FindById(comparisonObject.SourceObjectInternalName).TomPerspective, _targetTabularModel.Perspectives.FindById(comparisonObject.TargetObjectInternalName).TomPerspective);
                    OnValidationMessage(new ValidationMessageEventArgs($"Update Perspective [{comparisonObject.TargetObjectName}].", ValidationMessageType.Perspective, ValidationMessageStatus.Informational));
                }
            }

            #endregion

            #region Roles

            //Restore roles that were backed up earlier. Having done this there won't be any dependency issues, so can start comparison changes.
            _targetTabularModel.RestoreRoles();

            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Role && comparisonObject.MergeAction == MergeAction.Delete)
                {
                    _targetTabularModel.DeleteRole(comparisonObject.TargetObjectInternalName);
                    OnValidationMessage(new ValidationMessageEventArgs($"Delete Role [{comparisonObject.TargetObjectName}].", ValidationMessageType.Role, ValidationMessageStatus.Informational));
                }
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Role && comparisonObject.MergeAction == MergeAction.Create)
                {
                    _targetTabularModel.CreateRole(_sourceTabularModel.Roles.FindById(comparisonObject.SourceObjectInternalName).TomRole);
                    OnValidationMessage(new ValidationMessageEventArgs($"Create Role [{comparisonObject.SourceObjectName}].", ValidationMessageType.Role, ValidationMessageStatus.Informational));
                }
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Role && comparisonObject.MergeAction == MergeAction.Update)
                {
                    _targetTabularModel.UpdateRole(_sourceTabularModel.Roles.FindById(comparisonObject.SourceObjectInternalName), _targetTabularModel.Roles.FindById(comparisonObject.TargetObjectInternalName));
                    OnValidationMessage(new ValidationMessageEventArgs($"Update Role [{comparisonObject.TargetObjectName}].", ValidationMessageType.Role, ValidationMessageStatus.Informational));
                }
            }

            _targetTabularModel.RolesCleanup();

            #endregion

            #region Cultures

            //Restore cultures that were backed up earlier. Having done this there won't be any dependency issues, so can start comparison changes.
            //Note that cannot restore cultures before finished perspective comparison changes above, because cultures can have dependencies on perspectives.
            _targetTabularModel.RestoreCultues();

            // Do separate loops of _comparisonObjectects for Delete, Create, Update to ensure informational logging order is consistent with other object types. This also ensures deletions are done first to minimize chance of conflict.
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Culture && comparisonObject.MergeAction == MergeAction.Delete)
                {
                    _targetTabularModel.DeleteCulture(comparisonObject.TargetObjectInternalName);
                    OnValidationMessage(new ValidationMessageEventArgs($"Delete Culture [{comparisonObject.TargetObjectName}].", ValidationMessageType.Culture, ValidationMessageStatus.Informational));
                }
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Culture && comparisonObject.MergeAction == MergeAction.Create)
                {
                    _targetTabularModel.CreateCulture(_sourceTabularModel.Cultures.FindById(comparisonObject.SourceObjectInternalName).TomCulture);
                    OnValidationMessage(new ValidationMessageEventArgs($"Create Culture [{comparisonObject.SourceObjectName}].", ValidationMessageType.Culture, ValidationMessageStatus.Informational));
                }
            }
            foreach (ComparisonObject comparisonObject in _comparisonObjects)
            {
                if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Culture && comparisonObject.MergeAction == MergeAction.Update)
                {
                    _targetTabularModel.UpdateCulture(_sourceTabularModel.Cultures.FindById(comparisonObject.SourceObjectInternalName).TomCulture, _targetTabularModel.Cultures.FindById(comparisonObject.TargetObjectInternalName).TomCulture);
                    OnValidationMessage(new ValidationMessageEventArgs($"Update Culture [{comparisonObject.TargetObjectName}].", ValidationMessageType.Culture, ValidationMessageStatus.Informational));
                }
            }

            #endregion

            #endregion

            #region Missing measure dependencies

            if (_comparisonInfo.OptionsInfo.OptionMeasureDependencies)
            {
                foreach (Table table in _targetTabularModel.Tables)
                {
                    foreach (Measure measure in table.Measures)
                    {
                        foreach (string missingDependency in measure.FindMissingMeasureDependencies())
                        {
                            OnValidationMessage(new ValidationMessageEventArgs($"Measure [{measure.InternalName}] in table '{table.Name}' contains dependency on measure/column [{missingDependency}], which (considering changes to target) cannot be found in target model.", ValidationMessageType.MeasureCalculationDependency, ValidationMessageStatus.Warning));
                        }
                    }
                }
            }

            #endregion

            OnResizeValidationHeaders(new EventArgs());
        }

        #region Private methods for validation

        //Connections

        private void DeleteConnections(ComparisonObject comparisonObject)
        {
            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Connection && comparisonObject.MergeAction == MergeAction.Delete)
            {
                //Check any objects in target that depend on the connection are also going to be deleted
                string warningObjectList = CheckToDependencies(comparisonObject, MDependencyObjectType.Connection, "");

                if (warningObjectList == "")
                {
                    _targetTabularModel.DeleteConnection(comparisonObject.TargetObjectName);
                    OnValidationMessage(new ValidationMessageEventArgs($"Delete Connection [{comparisonObject.TargetObjectName}].", ValidationMessageType.Connection, ValidationMessageStatus.Informational));
                }
                else
                {
                    OnValidationMessage(new ValidationMessageEventArgs($"Unable to delete connection {comparisonObject.TargetObjectName} because the following objects depend on it, and (considering changes) are not being deleted {warningObjectList.Substring(0, warningObjectList.Length - 2)}.", ValidationMessageType.Connection, ValidationMessageStatus.Warning));
                }
            }
        }

        private void CreateConnections(ComparisonObject comparisonObject)
        {
            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Connection && comparisonObject.MergeAction == MergeAction.Create)
            {
                _targetTabularModel.CreateConnection(_sourceTabularModel.Connections.FindByName(comparisonObject.SourceObjectName));
                OnValidationMessage(new ValidationMessageEventArgs($"Create Connection [{comparisonObject.SourceObjectName}].", ValidationMessageType.Connection, ValidationMessageStatus.Informational));
            }
        }

        private void UpdateConnections(ComparisonObject comparisonObject)
        {
            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Connection && comparisonObject.MergeAction == MergeAction.Update)
            {
                _targetTabularModel.UpdateConnection(_sourceTabularModel.Connections.FindByName(comparisonObject.SourceObjectName), _targetTabularModel.Connections.FindByName(comparisonObject.TargetObjectName));
                OnValidationMessage(new ValidationMessageEventArgs($"Update Connection [{comparisonObject.TargetObjectName}].", ValidationMessageType.Connection, ValidationMessageStatus.Informational));
            }
        }

        //Expressions

        private void DeleteExpressions(ComparisonObject comparisonObject)
        {
            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Expression && comparisonObject.MergeAction == MergeAction.Delete)
            {
                //Check any objects in target that depend on the connection are also going to be deleted
                string warningObjectList = CheckToDependencies(comparisonObject, MDependencyObjectType.Expression, "");

                if (warningObjectList == "")
                {
                    _targetTabularModel.DeleteExpression(comparisonObject.TargetObjectName);
                    OnValidationMessage(new ValidationMessageEventArgs($"Delete Expresssion [{comparisonObject.TargetObjectName}].", ValidationMessageType.Expression, ValidationMessageStatus.Informational));
                }
                else
                {
                    OnValidationMessage(new ValidationMessageEventArgs($"Unable to delete expression {comparisonObject.TargetObjectName} because the following objects depend on it, and (considering changes) are not being deleted {warningObjectList.Substring(0, warningObjectList.Length - 2)}.", ValidationMessageType.Expression, ValidationMessageStatus.Warning));
                }
            }
        }

        private void CreateExpressions(ComparisonObject comparisonObject)
        {
            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Expression && comparisonObject.MergeAction == MergeAction.Create)
            {
                //Check any objects in source that this expression depends on are also going to be created if not already in target
                string warningObjectList = CheckFromDependencies(comparisonObject, MDependencyObjectType.Expression, "");

                if (warningObjectList == "")
                {
                    _targetTabularModel.CreateExpression(_sourceTabularModel.Expressions.FindByName(comparisonObject.SourceObjectName).TomExpression);
                    OnValidationMessage(new ValidationMessageEventArgs($"Create Expresssion [{comparisonObject.SourceObjectName}].", ValidationMessageType.Expression, ValidationMessageStatus.Informational));
                }
                else
                {
                    OnValidationMessage(new ValidationMessageEventArgs($"Unable to create expression {comparisonObject.SourceObjectName} because it depends on the following objects, which (considering changes) are missing and not being created {warningObjectList.Substring(0, warningObjectList.Length - 2)}.", ValidationMessageType.Expression, ValidationMessageStatus.Warning));
                }
            }
        }

        private void UpdateExpressions(ComparisonObject comparisonObject)
        {
            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Expression && comparisonObject.MergeAction == MergeAction.Update)
            {
                //Check any objects in source that this expression depends on are also going to be created if not already in target
                string warningObjectList = CheckFromDependencies(comparisonObject, MDependencyObjectType.Expression, "");

                if (warningObjectList == "")
                {
                    _targetTabularModel.UpdateExpression(_sourceTabularModel.Expressions.FindByName(comparisonObject.SourceObjectName), _targetTabularModel.Expressions.FindByName(comparisonObject.TargetObjectName));
                    OnValidationMessage(new ValidationMessageEventArgs($"Update Expression [{comparisonObject.TargetObjectName}].", ValidationMessageType.Expression, ValidationMessageStatus.Informational));
                }
                else
                {
                    OnValidationMessage(new ValidationMessageEventArgs($"Unable to update expression {comparisonObject.TargetObjectName} because version from the source depends on the following objects, which (considering changes) are missing and not being created {warningObjectList.Substring(0, warningObjectList.Length - 2)}.", ValidationMessageType.Expression, ValidationMessageStatus.Warning));
                }
            }
        }

        //M Dependency checking

        private string CheckToDependencies(ComparisonObject comparisonObject, MDependencyObjectType referencedObjectType, string warningObjectList)
        {
            //For deletion.
            //Check any objects in target that depend on this object are also going to be deleted.

            MDependencyCollection targetToDepdendencies = _targetTabularModel.MDependencies.DependenciesReferenceTo(referencedObjectType, comparisonObject.TargetObjectName);
            foreach (MDependency targetToDependency in targetToDepdendencies)
            {
                switch (targetToDependency.ObjectType)
                {
                    case MDependencyObjectType.Expression:
                        foreach (ComparisonObject comparisonObjectCheck in _comparisonObjects)
                        {
                            if (comparisonObjectCheck.ComparisonObjectType == ComparisonObjectType.Expression &&
                                comparisonObjectCheck.TargetObjectName == targetToDependency.ObjectName &&
                                comparisonObjectCheck.Status == ComparisonObjectStatus.MissingInSource &&
                                comparisonObjectCheck.MergeAction == MergeAction.Skip)
                            {
                                warningObjectList += $"Expression {comparisonObjectCheck.TargetObjectName}, ";
                            }
                        }
                        break;
                    case MDependencyObjectType.Partition:
                        foreach (ComparisonObject comparisonObjectToCheck in _comparisonObjects)
                        {
                            warningObjectList = CheckPartitionToDependencies(comparisonObjectToCheck, targetToDependency, warningObjectList);
                        }
                        break;
                    default:
                        break;
                }
            }

            return warningObjectList;
        }

        private string CheckPartitionToDependencies(ComparisonObject comparisonObjectCheck, MDependency targetToDependency, string warningObjectList)
        {
            if (comparisonObjectCheck.ComparisonObjectType == ComparisonObjectType.Table &&
                comparisonObjectCheck.TargetObjectName == targetToDependency.TableName &&
                comparisonObjectCheck.Status == ComparisonObjectStatus.MissingInTarget &&
                comparisonObjectCheck.MergeAction == MergeAction.Skip)
            {
                warningObjectList += $"Table {comparisonObjectCheck.TargetObjectName}/Partition {targetToDependency.ObjectName}, ";
            }

            foreach (ComparisonObject comparisonChildObjectCheck in comparisonObjectCheck.ChildComparisonObjects)
            {
                //Need a recursive call to cover tables extra level deep under connections
                warningObjectList = CheckPartitionToDependencies(comparisonChildObjectCheck, targetToDependency, warningObjectList);
            }

            return warningObjectList;
        }

        private string CheckFromDependencies(ComparisonObject comparisonObject, MDependencyObjectType objectType, string warningObjectList)
        {
            //For creation and updates.
            //Check any objects in source that this object depends on are also going to be created if not already in target.

            MDependencyCollection sourceFromDepdendencies = _sourceTabularModel.MDependencies.DependenciesReferenceFrom(objectType, comparisonObject.SourceObjectName);
            foreach (MDependency sourceFromDependency in sourceFromDepdendencies)
            {
                switch (sourceFromDependency.ReferencedObjectType)
                {
                    case MDependencyObjectType.Expression:
                        if (!_targetTabularModel.Expressions.ContainsName(sourceFromDependency.ReferencedObjectName))
                        {
                            foreach (ComparisonObject comparisonObjectToCheck in _comparisonObjects)
                            {
                                if (comparisonObjectToCheck.ComparisonObjectType == ComparisonObjectType.Expression &&
                                    comparisonObjectToCheck.SourceObjectName == sourceFromDependency.ReferencedObjectName &&
                                    comparisonObjectToCheck.Status == ComparisonObjectStatus.MissingInTarget &&
                                    comparisonObjectToCheck.MergeAction == MergeAction.Skip)
                                {
                                    warningObjectList += $"Expression {comparisonObjectToCheck.SourceObjectName}, ";
                                }
                            }
                        }
                        break;
                    case MDependencyObjectType.Connection:
                        if (!_targetTabularModel.Connections.ContainsName(sourceFromDependency.ReferencedObjectName))
                        {
                            foreach (ComparisonObject comparisonObjectToCheck in _comparisonObjects)
                            {
                                if (comparisonObjectToCheck.ComparisonObjectType == ComparisonObjectType.Connection &&
                                    comparisonObjectToCheck.SourceObjectName == sourceFromDependency.ReferencedObjectName &&
                                    comparisonObjectToCheck.Status == ComparisonObjectStatus.MissingInTarget &&
                                    comparisonObjectToCheck.MergeAction == MergeAction.Skip)
                                {
                                    warningObjectList += $"Connection {comparisonObjectToCheck.SourceObjectName}, ";
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return warningObjectList;
        }

        //Tables

        private void DeleteTables(ComparisonObject comparisonObject)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                //tables with a connection will be extra level deep
                DeleteTables(childComparisonObject);
            }

            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Table && comparisonObject.MergeAction == MergeAction.Delete)
            {
                _targetTabularModel.DeleteTable(comparisonObject.TargetObjectName);
                OnValidationMessage(new ValidationMessageEventArgs($"Delete Table '{comparisonObject.TargetObjectName}'.", ValidationMessageType.Table, ValidationMessageStatus.Informational));
            }
        }

        private void CreateTables(ComparisonObject comparisonObject)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                //tables with a connection will be extra level deep
                CreateTables(childComparisonObject);
            }

            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Table && comparisonObject.MergeAction == MergeAction.Create)
            {
                Table tableTarget = _targetTabularModel.Tables.FindByName(comparisonObject.SourceObjectName);
                if (tableTarget == null)
                {
                    _targetTabularModel.CreateTable(_sourceTabularModel.Tables.FindByName(comparisonObject.SourceObjectName));
                    OnValidationMessage(new ValidationMessageEventArgs($"Create Table '{comparisonObject.SourceObjectName}'.", ValidationMessageType.Table, ValidationMessageStatus.Informational));
                }
                else
                {
                    OnValidationMessage(new ValidationMessageEventArgs($"Unable to create Table {comparisonObject.SourceObjectName} because another table with the same name (under a connection) already exists in target model.", ValidationMessageType.Table, ValidationMessageStatus.Warning));
                }
            }
        }

        private void UpdateTables(ComparisonObject comparisonObject)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                //tables with a connection will be extra level deep
                UpdateTables(childComparisonObject);
            }

            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Table && comparisonObject.MergeAction == MergeAction.Update)
            {
                _targetTabularModel.UpdateTable(_sourceTabularModel.Tables.FindByName(comparisonObject.SourceObjectName), _targetTabularModel.Tables.FindByName(comparisonObject.TargetObjectName));
                OnValidationMessage(new ValidationMessageEventArgs($"Update Table '{comparisonObject.TargetObjectName}'.", ValidationMessageType.Table, ValidationMessageStatus.Informational));
            }
        }

        //Relationships

        private void DeleteRelationships(ComparisonObject comparisonObject)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                DeleteRelationships(childComparisonObject);
            }

            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Relationship && comparisonObject.MergeAction == MergeAction.Delete)
            {
                foreach (Table tableTarget in _targetTabularModel.Tables)
                {
                    Relationship relationshipTarget = tableTarget.Relationships.FindByName(comparisonObject.TargetObjectName.Trim());

                    if (relationshipTarget != null)
                    {
                        // Relationship may have already been deleted if parent table was deleted
                        tableTarget.DeleteRelationship(comparisonObject.TargetObjectInternalName);
                        break;
                    }
                }

                OnValidationMessage(new ValidationMessageEventArgs($"Delete Relationship {comparisonObject.TargetObjectName.Trim()}.", ValidationMessageType.Relationship, ValidationMessageStatus.Informational));
            }
        }

        private void CreateRelationships(ComparisonObject comparisonObject, string tableName)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                CreateRelationships(childComparisonObject, comparisonObject.SourceObjectName);
            }

            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Relationship && comparisonObject.MergeAction == MergeAction.Create)
            {
                Table tableSource = _sourceTabularModel.Tables.FindByName(tableName);
                Table tableTarget = _targetTabularModel.Tables.FindByName(tableName);
                Relationship relationshipSource = tableSource.Relationships.FindByInternalName(comparisonObject.SourceObjectInternalName);
                Table parentTableSource = _sourceTabularModel.Tables.FindByName(relationshipSource.ToTableName);

                string warningMessage = $"Unable to create Relationship {comparisonObject.SourceObjectName.Trim()} because (considering changes) necessary table/column(s) not found in target model.";
                if (tableTarget != null && tableTarget.CreateRelationshipWithValidation(relationshipSource, parentTableSource.TomTable, comparisonObject.SourceObjectName.Trim(), ref warningMessage))
                {
                    OnValidationMessage(new ValidationMessageEventArgs($"Create Relationship {comparisonObject.SourceObjectName.Trim()}.", ValidationMessageType.Relationship, ValidationMessageStatus.Informational));
                }
                else
                {
                    OnValidationMessage(new ValidationMessageEventArgs(warningMessage, ValidationMessageType.Relationship, ValidationMessageStatus.Warning));
                }
            }
        }

        private void UpdateRelationships(ComparisonObject comparisonObject, string tableName)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                UpdateRelationships(childComparisonObject, comparisonObject.SourceObjectName);
            }

            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Relationship && comparisonObject.MergeAction == MergeAction.Update)
            {
                Table tableSource = _sourceTabularModel.Tables.FindByName(tableName);
                Table tableTarget = _targetTabularModel.Tables.FindByName(tableName);
                Relationship relationshipSource = tableSource.Relationships.FindByInternalName(comparisonObject.SourceObjectInternalName);
                Table parentTableSource = _sourceTabularModel.Tables.FindByName(relationshipSource.ToTableName);

                string warningMessage = "";
                if (tableTarget.UpdateRelationship(relationshipSource, parentTableSource.TomTable, comparisonObject.SourceObjectName.Trim(), ref warningMessage))
                {
                    OnValidationMessage(new ValidationMessageEventArgs($"Update Relationship {comparisonObject.SourceObjectName.Trim()}.", ValidationMessageType.Relationship, ValidationMessageStatus.Informational));
                }
                else
                {
                    OnValidationMessage(new ValidationMessageEventArgs(warningMessage, ValidationMessageType.Relationship, ValidationMessageStatus.Warning));
                }
            }
        }

        //Measures / KPIs

        private void DeleteMeasures(ComparisonObject comparisonObject)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                DeleteMeasures(childComparisonObject);
            }

            if ((comparisonObject.ComparisonObjectType == ComparisonObjectType.Measure || comparisonObject.ComparisonObjectType == ComparisonObjectType.Kpi) && 
                 comparisonObject.MergeAction == MergeAction.Delete)
            {
                foreach (Table tableTarget in _targetTabularModel.Tables)
                {
                    Measure measureTarget = tableTarget.Measures.FindByName(comparisonObject.TargetObjectInternalName);

                    if (measureTarget != null)
                    {
                        // Measure may have already been deleted if parent table was deleted
                        tableTarget.DeleteMeasure(comparisonObject.TargetObjectInternalName);
                        break;
                    }
                }

                OnValidationMessage(new ValidationMessageEventArgs($"Delete Measure / KPI {comparisonObject.TargetObjectInternalName}.", ValidationMessageType.Measure, ValidationMessageStatus.Informational));
            }
        }

        private void CreateMeasures(ComparisonObject comparisonObject, string tableName)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                CreateMeasures(childComparisonObject, comparisonObject.SourceObjectName);
            }

            if ((comparisonObject.ComparisonObjectType == ComparisonObjectType.Measure || comparisonObject.ComparisonObjectType == ComparisonObjectType.Kpi) && 
                 comparisonObject.MergeAction == MergeAction.Create)
            {
                foreach (Table tableInTarget in _targetTabularModel.Tables)
                {
                    Measure measureInTarget = tableInTarget.Measures.FindByName(comparisonObject.SourceObjectInternalName);

                    if (measureInTarget != null)
                    {
                        OnValidationMessage(new ValidationMessageEventArgs($"Unable to create Measure / KPI {comparisonObject.SourceObjectInternalName} because name already exists in target model.", ValidationMessageType.Measure, ValidationMessageStatus.Warning));
                        return;
                    }
                }

                //If we get here, can create measure/kpi
                Table tableSource = _sourceTabularModel.Tables.FindByName(tableName);
                Table tableTarget = _targetTabularModel.Tables.FindByName(tableName);
                Measure measureSource = tableSource.Measures.FindByName(comparisonObject.SourceObjectInternalName);

                tableTarget.CreateMeasure(measureSource.TomMeasure);
                OnValidationMessage(new ValidationMessageEventArgs($"Create Measure / KPI {comparisonObject.SourceObjectInternalName}.", ValidationMessageType.Measure, ValidationMessageStatus.Informational));
            }
        }

        private void UpdateMeasures(ComparisonObject comparisonObject, string tableName)
        {
            foreach (ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                UpdateMeasures(childComparisonObject, comparisonObject.SourceObjectName);
            }

            if ((comparisonObject.ComparisonObjectType == ComparisonObjectType.Measure || comparisonObject.ComparisonObjectType == ComparisonObjectType.Kpi) &&
                 comparisonObject.MergeAction == MergeAction.Update)
            {
                Table tableSource = _sourceTabularModel.Tables.FindByName(tableName);
                Table tableTarget = _targetTabularModel.Tables.FindByName(tableName);
                Measure measureSource = tableSource.Measures.FindByName(comparisonObject.SourceObjectInternalName);

                tableTarget.UpdateMeasure(measureSource.TomMeasure);
                OnValidationMessage(new ValidationMessageEventArgs($"Update Measure / KPI {comparisonObject.SourceObjectInternalName}.", ValidationMessageType.Measure, ValidationMessageStatus.Informational));
            }
        }

        #endregion

        /// <summary>
        /// Update target tabular model with changes defined by actions in ComparisonObject instances.
        /// </summary>
        /// <returns>Flag to indicate whether update was successful.</returns>
        public override bool Update() => _targetTabularModel.Update();

        /// <summary>
        /// Gets a collection of ProcessingTable objects depending on Process Affected Tables option.
        /// </summary>
        /// <returns>Collection of ProcessingTable objects.</returns>
        public override ProcessingTableCollection GetTablesToProcess()
        {
            ProcessingTableCollection tablesToProcess = new ProcessingTableCollection();

            if (_comparisonInfo.OptionsInfo.OptionProcessingOption != ProcessingOption.DoNotProcess)
            {
                if (_comparisonInfo.OptionsInfo.OptionAffectedTables)
                {
                    foreach (Core.ComparisonObject comparisonObject in _comparisonObjects)
                    {
                        ProcessAffectedTables(comparisonObject, tablesToProcess);
                    }
                }
                else
                {
                    foreach (Table table in _targetTabularModel.Tables)
                    {
                        tablesToProcess.Add(new ProcessingTable(table.Name, table.InternalName));
                    }
                }
            }

            tablesToProcess.Sort();
            return tablesToProcess;
        }

        private void ProcessAffectedTables(Core.ComparisonObject comparisonObject, ProcessingTableCollection tablesToProcess)
        {
            //Recursively call for multiple levels to ensure catch calculated tables or those child of connection

            if (comparisonObject.ComparisonObjectType == ComparisonObjectType.Table &&
                (comparisonObject.MergeAction == MergeAction.Create || comparisonObject.MergeAction == MergeAction.Update)
               )
            {
                tablesToProcess.Add(new ProcessingTable(comparisonObject.SourceObjectName, comparisonObject.SourceObjectInternalName));
            }

            foreach (Core.ComparisonObject childComparisonObject in comparisonObject.ChildComparisonObjects)
            {
                ProcessAffectedTables(childComparisonObject, tablesToProcess);
            }
        }

        /// <summary>
        /// Deploy database to target server and perform processing if required.
        /// </summary>
        /// <param name="tablesToProcess"></param>
        public override void DatabaseDeployAndProcess(ProcessingTableCollection tablesToProcess)
        {
            _targetTabularModel.DatabaseDeployAndProcess(tablesToProcess);
        }

        /// <summary>
        /// Stop processing of deployed database.
        /// </summary>
        public override void StopProcessing()
        {
            _targetTabularModel.StopProcessing();
        }

        /// <summary>
        /// Generate script of target database including changes.
        /// </summary>
        /// <returns>Script.</returns>
        public override string ScriptDatabase() => _targetTabularModel.ScriptDatabase();

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_sourceTabularModel != null)
                    {
                        _sourceTabularModel.Dispose();
                    }
                    if (_targetTabularModel != null)
                    {
                        _targetTabularModel.Dispose();
                    }
                }

                _disposed = true;
            }
        }

    }
}
