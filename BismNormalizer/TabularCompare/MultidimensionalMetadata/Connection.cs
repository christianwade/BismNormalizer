using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.AnalysisServices;

namespace BismNormalizer.TabularCompare.MultidimensionalMetadata
{
    /// <summary>
    /// Abstraction of a tabular model connection with properties and methods for comparison purposes.
    /// </summary>
    public class Connection : ITabularObject
    {
        private TabularModel _parentTabularModel;
        private DataSource _amoDatasource;
        private string _substituteId;

        /// <summary>
        /// Initializes a new instance of the Connection class using multiple parameters.
        /// </summary>
        /// <param name="parentTabularModel">TabularModel object that the Connection object belongs to.</param>
        /// <param name="datasource">Analysis Management Objects DataSource object abtstracted by the Connection object.</param>
        public Connection(TabularModel parentTabularModel, DataSource datasource)
        {
            _parentTabularModel = parentTabularModel;
            _amoDatasource = datasource;
        }

        /// <summary>
        /// TabularModel object that the Connection object belongs to.
        /// </summary>
        public TabularModel ParentTabularModel => _parentTabularModel;

        /// <summary>
        /// Analysis Management Objects DataSource object abtstracted by the Connection object.
        /// </summary>
        public DataSource AmoDatasource => _amoDatasource;

        /// <summary>
        /// Name of the Connection object.
        /// </summary>
        public string Name => _amoDatasource.Name;

        /// <summary>
        /// Long name of the Connection object.
        /// </summary>
        public string LongName => _amoDatasource.Name;

        /// <summary>
        /// Id of the Connection object.
        /// </summary>
        public string Id => _amoDatasource.ID;

        /// <summary>
        /// Object definition of the Connection object. This is a simplified list of relevant attribute values for comparison; not the XMLA definition of the abstracted AMO object.
        /// </summary>
        public string ObjectDefinition
        {
            get
            {
                //the order of items in the connection string is not guaranteed to come out in a consistent order ...
                string[] elements = _amoDatasource.ConnectionString.Split(';');
                Array.Sort(elements);
                string returnValue = string.Empty;
                foreach (string element in elements)
                {
                    returnValue += element + ";";
                }
                return returnValue.Substring(0, returnValue.Length - 1) + "\n";
            }
        }

        /// <summary>
        /// Substitute Id of the Connection object.
        /// </summary>
        public string SubstituteId
        {
            get
            {
                if (string.IsNullOrEmpty(_substituteId))
                {
                    return _amoDatasource.ID;
                }
                else
                {
                    return _substituteId;
                }
            }
            set
            {
                _substituteId = value;
            }
        }

        public override string ToString() => this.GetType().FullName;
    }
}
