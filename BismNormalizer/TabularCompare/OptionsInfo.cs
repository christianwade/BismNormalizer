﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BismNormalizer.TabularCompare
{
    /// <summary>
    /// Information about options for a comparison. This is serialized/deserialized to/from the BSMN file.
    /// </summary>
    public class OptionsInfo
    {
        private bool _optionPerspectives;
        private bool _optionMergePerspectives;
        private bool _optionCultures;
        private bool _optionMergeCultures;
        private bool _optionRoles;
        private bool _optionRetainRoleMembers;
        private bool _optionActions;
        private bool _optionPartitions;
        private bool _optionLineageTag;
        private bool _optionRetainPartitions;
        private bool _optionRetainPolicyPartitions;
        private bool _optionRetainRefreshPolicy;
        private bool _optionRetainStorageMode;
        private bool _optionMeasureDependencies;
        private ProcessingOption _optionProcessingOption;
        private bool _optionTransaction;
        private bool _optionAffectedTables;

        /// <summary>
        /// Initializes a new instance of the OptionsInfo class.
        /// </summary>
        public OptionsInfo()
        {
            _optionPerspectives = Settings.Default.OptionPerspectives;
            _optionMergePerspectives = Settings.Default.OptionMergePerspectives;
            _optionCultures = Settings.Default.OptionCultures;
            _optionMergeCultures = Settings.Default.OptionMergeCultures;
            _optionRoles = Settings.Default.OptionRoles;
            _optionRetainRoleMembers = Settings.Default.OptionRetainRoleMembers;
            _optionActions = Settings.Default.OptionActions;
            _optionPartitions = Settings.Default.OptionPartitions;
            _optionLineageTag = Settings.Default.OptionLineageTag;
            _optionRetainPartitions = Settings.Default.OptionRetainPartitions;
            _optionRetainPolicyPartitions = Settings.Default.OptionRetainPolicyPartitions;
            _optionRetainRefreshPolicy = Settings.Default.OptionRetainRefreshPolicy;
            _optionRetainStorageMode = Settings.Default.OptionRetainStorageMode;
            _optionMeasureDependencies = Settings.Default.OptionMeasureDependencies;
            _optionProcessingOption = (ProcessingOption)Enum.Parse(typeof(ProcessingOption), Settings.Default.OptionProcessingOption);
            _optionTransaction = Settings.Default.OptionTransaction;
        }

        /// <summary>
        /// A Boolean specifying whether to include perspectives in the comparison.
        /// </summary>
        public bool OptionPerspectives
        {
            get { return _optionPerspectives; }
            set { _optionPerspectives = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to merge perspective selections when updating, rather than replace.
        /// </summary>
        public bool OptionMergePerspectives
        {
            get { return _optionMergePerspectives; }
            set { _optionMergePerspectives = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to include cultures in the comparison. This is ignored by tabular models with multidimensional metadata as they don't support cultures.
        /// </summary>
        public bool OptionCultures
        {
            get { return _optionCultures; }
            set { _optionCultures = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to merge culture translations when updating, rather than replace.
        /// </summary>
        public bool OptionMergeCultures
        {
            get { return _optionMergeCultures; }
            set { _optionMergeCultures = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to include roles in the comparison.
        /// </summary>
        public bool OptionRoles
        {
            get { return _optionRoles; }
            set { _optionRoles = value; }
        }

        public bool OptionRetainRoleMembers 
        {
            get { return _optionRetainRoleMembers; }
            set { _optionRetainRoleMembers = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to include actions in the comparison.
        /// </summary>
        [XmlIgnore()]
        //[Obsolete("This property is obsolete. Left over from BISM Normalizer 2, which supported BIDS Helper actions.")]
        public bool OptionActions
        {
            get { return _optionActions; }
            set { _optionActions = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to consider partitions when comparing tables.
        /// </summary>
        public bool OptionPartitions
        {
            get { return _optionPartitions; }
            set { _optionPartitions = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to consider LineageTag when comparing objects.
        /// </summary>
        public bool OptionLineageTag
        {
            get { return _optionLineageTag; }
            set { _optionLineageTag = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to retain partitions for table updates.
        /// </summary>
        public bool OptionRetainPartitions
        {
            get { return _optionRetainPartitions; }
            set { _optionRetainPartitions = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to retain refresh-policy partitions for table updates.
        /// </summary>
        public bool OptionRetainPolicyPartitions
        {
            get { return _optionRetainPolicyPartitions; }
            set { _optionRetainPolicyPartitions = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to retain incremental refresh policy.
        /// </summary>
        public bool OptionRetainRefreshPolicy
        {
            get { return _optionRetainRefreshPolicy; }
            set { _optionRetainRefreshPolicy = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to retain storage for table updates on composite models.
        /// </summary>
        public bool OptionRetainStorageMode
        {
            get { return _optionRetainStorageMode; }
            set { _optionRetainStorageMode = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to display warnings for missing measure dependencies.
        /// </summary>
        public bool OptionMeasureDependencies
        {
            get { return _optionMeasureDependencies; }
            set { _optionMeasureDependencies = value; }
        }

        /// <summary>
        /// Processing option for database deployment.
        /// </summary>
        public ProcessingOption OptionProcessingOption
        {
            get { return _optionProcessingOption; }
            set { _optionProcessingOption = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to deploy within a transaction.
        /// </summary>
        [XmlIgnore()]
        //[Obsolete("This property is obsolete. Do not use. Left over from a previous version that supported transactions.")]
        public bool OptionTransaction
        {
            get { return _optionTransaction; }
            set { _optionTransaction = value; }
        }

        /// <summary>
        /// A Boolean specifying whether to process only the tables affected by the comparison.
        /// </summary>
        public bool OptionAffectedTables
        {
            get { return _optionAffectedTables; }
            set { _optionAffectedTables = value; }
        }

        /// <summary>
        /// Save options as default selections for new comparisons that are not deserialized from BSMN file.
        /// </summary>
        public void Save()
        {
            Settings.Default.OptionPerspectives = _optionPerspectives;
            Settings.Default.OptionMergePerspectives = _optionMergePerspectives;
            Settings.Default.OptionCultures = _optionCultures;
            Settings.Default.OptionMergeCultures = _optionMergeCultures;
            Settings.Default.OptionRoles = _optionRoles;
            Settings.Default.OptionRetainRoleMembers = _optionRetainRoleMembers;
            Settings.Default.OptionActions = _optionActions;
            Settings.Default.OptionPartitions = _optionPartitions;
            Settings.Default.OptionLineageTag = _optionLineageTag;
            Settings.Default.OptionRetainPartitions = _optionRetainPartitions;
            Settings.Default.OptionRetainPolicyPartitions = _optionRetainPolicyPartitions;
            Settings.Default.OptionRetainRefreshPolicy = _optionRetainRefreshPolicy;
            Settings.Default.OptionRetainStorageMode = _optionRetainStorageMode;
            Settings.Default.OptionMeasureDependencies = _optionMeasureDependencies;
            Settings.Default.OptionProcessingOption = _optionProcessingOption.ToString();
            Settings.Default.OptionTransaction = _optionTransaction;
            Settings.Default.OptionAffectedTables = _optionAffectedTables;

            Settings.Default.Save();
        }

    }
}
