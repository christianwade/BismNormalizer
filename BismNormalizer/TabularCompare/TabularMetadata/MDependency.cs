using System;
using System.Collections.Generic;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Dependency between partitions, M expressions and data sources
    /// </summary>
    public class MDependency
    {
        private TabularModel _parentTabularModel;
        private string _objectType;
        private string _tableName;
        private string _objectName;
        private string _expression;
        private string _referencedObjectType;
        private string _referencedObjectName;
        private string _referencedExpression;

        /// <summary>
        /// Initializes a new instance of an MDependency class using multiple parameters    .
        /// </summary>
        /// <param name="parentTabularModel">TabularModel object that the MDependency object belongs to.</param>
        public MDependency(TabularModel parentTabularModel, string objectType, string tableName, string objectName, string expression, string referencedObjectType, string referencedObjectName, string referencedExpression)
        {
            _parentTabularModel = parentTabularModel;
            _objectType = objectType;
            _tableName = tableName;
            _objectName = objectName;
            _expression = expression;
            _referencedObjectType = referencedObjectType;
            _referencedObjectName = referencedObjectName;
            _referencedExpression = referencedExpression;

            //cbw todo delete:
            if (_objectName == "FactInternetSales" && _expression.Contains("#\"Filtered Rows\""))
            {
                _objectName = "InternetSalesFiltered";
            }
            if (_objectName == "FactInternetSales" && _expression.Contains("#\"Merged Queries\""))
            {
                _objectName = "InternetSalesMerged";
            }
        }

        /// <summary>
        /// The object type of the dependency.
        /// </summary>
        public string ObjectType => _objectType;
        
        /// <summary>
        /// The table name of the dependency.
        /// </summary>
        public string TableName => _tableName;
        
        /// <summary>
        /// The object name of the dependency.
        /// </summary>
        public string ObjectName => _objectName;

        /// <summary>
        /// The expression of the dependency.
        /// </summary>
        public string Expression => _expression;
        
        /// <summary>
        /// The referenced object type of the dependency.
        /// </summary>
        public string ReferencedObjectType => _referencedObjectType;

        /// <summary>
        /// The referenced object name of the dependency.
        /// </summary>
        public string ReferencedObjectName => _referencedObjectName;

        /// <summary>
        /// The referenced expression of the dependency.
        /// </summary>
        public string ReferencedExpression => _referencedExpression;

        public override string ToString() => this.GetType().FullName;
    }
}
