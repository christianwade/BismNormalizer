using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AnalysisServices.Tabular;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Represents a collection of Table objects.
    /// </summary>
    public class TableCollection : List<Table>
    {
        /// <summary>
        /// Find an object in the collection by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Table object if found. Null if not found.</returns>
        public Table FindByName(string name)
        {
            foreach (Table table in this)
            {
                if (table.Name == name)
                {
                    return table;
                }
            }
            return null;
        }

        /// <summary>
        /// A Boolean specifying whether the collection contains object by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the object is found, or False if it's not found.</returns>
        public bool ContainsName(string name)
        {
            foreach (Table table in this)
            {
                if (table.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a collection of Table objects filtered by the parent connection's name.
        /// </summary>
        /// <param name="dataSourceName"></param>
        /// <returns>TableCollection</returns>
        public TableCollection FilterByConnection(DataSource dataSource)
        {
            TableCollection returnTables = new TableCollection();
            foreach (Table table in this)
            {
                if (dataSource is ProviderDataSource && table.ConnectionName == dataSource.Name)
                {
                    returnTables.Add(table);
                }
                else if (dataSource is StructuredDataSource && table.TomTable.Partitions.Count > 0)
                {
                    bool partitionsMatch = false;
                    
                    //Check all the partitions refer to the connection, otherwise don't consider as child of the connection
                    foreach (Partition partition in table.TomTable.Partitions)
                    {
                        if ( partition.SourceType == PartitionSourceType.M &&
                             ((MPartitionSource)partition.Source).Expression.Replace(" ", string.Empty).Contains($"Source={dataSource.Name},"))
                        {
                            partitionsMatch = true;
                        }
                        else
                        {
                            partitionsMatch = false;
                            break;
                        }
                    }

                    if (partitionsMatch)
                    {
                        returnTables.Add(table);
                    }
                }
            }
            return returnTables;
        }

        /// <summary>
        /// Returns a collection of Table objects that do not have a connection associated with them. These can be calculated tables or tables with M partitions that do not refer to a connection.
        /// </summary>
        /// <returns></returns>
        public TableCollection WithoutConnection(Model model)
        {
            TableCollection tablesWithConnection = new TableCollection();
            foreach (DataSource dataSource in model.DataSources)
            {
                tablesWithConnection.AddRange(this.FilterByConnection(dataSource));
            }

            TableCollection tablesWithoutConnection = new TableCollection();
            foreach (Table table in this)
            {
                if (!tablesWithConnection.ContainsName(table.Name))
                {
                    tablesWithoutConnection.Add(table);
                }
            }

            return tablesWithoutConnection;
        }

        /// <summary>
        /// Removes an object from the collection by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the object was removed, or False if was not found.</returns>
        public bool RemoveByName(string name)
        {
            foreach (Table table in this)
            {
                if (table.Name == name)
                {
                    this.Remove(table);
                    return true;
                }
            }
            return false;
        }
    }
}
