using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public TableCollection FilterByConnectionName(string dataSourceName)
        {
            TableCollection returnTables = new TableCollection();
            foreach (Table table in this)
            {
                if (table.ConnectionName == dataSourceName)
                {
                    returnTables.Add(table);
                }
            }
            return returnTables;
        }

        /// <summary>
        /// Returns a collection of Table objects that do not have a connection associated with them. These are calculated tables.
        /// </summary>
        /// <returns></returns>
        public TableCollection WithoutConnection()
        {
            TableCollection returnTables = new TableCollection();
            foreach (Table table in this)
            {
                if (table.ConnectionName == "")
                {
                    returnTables.Add(table);
                }
            }
            return returnTables;
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
