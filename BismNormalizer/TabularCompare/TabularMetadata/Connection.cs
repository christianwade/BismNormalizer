using System;
using System.Collections.Generic;
using Microsoft.AnalysisServices.Tabular;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Abstraction of a tabular model connection with properties and methods for comparison purposes.
    /// </summary>
    public class Connection : TabularObject
    {
        private TabularModel _parentTabularModel;
        private DataSource _tomConnection;

        /// <summary>
        /// Initializes a new instance of the Connection class using multiple parameters.
        /// </summary>
        /// <param name="parentTabularModel">TabularModel object that the Connection object belongs to.</param>
        /// <param name="datasource">Tabular Object Model ProviderDataSource object abtstracted by the Connection class.</param>
        public Connection(TabularModel parentTabularModel, DataSource dataSource) : base(dataSource)
        {
            _parentTabularModel = parentTabularModel;
            _tomConnection = dataSource;
        }

        /// <summary>
        /// TabularModel object that the Connection object belongs to.
        /// </summary>
        public TabularModel ParentTabularModel => _parentTabularModel;

        /// <summary>
        /// Tabular Object Model ProviderDataSource object abtstracted by the Connection class.
        /// </summary>
        public DataSource TomConnection => _tomConnection;

        public override string ToString() => this.GetType().FullName;
    }
}
