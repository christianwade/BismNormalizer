using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Represents a collection of Connection objects.
    /// </summary>
    public class ConnectionCollection : List<Connection>
    {
        /// <summary>
        /// Find an object in the collection by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Connection object if found. Null if not found.</returns>
        public Connection FindByName(string name)
        {
            foreach (Connection connection in this)
            {
                if (connection.Name == name)
                {
                    return connection;
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
            foreach (Connection connection in this)
            {
                if (connection.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes an object from the collection by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the object was removed, or False if was not found.</returns>
        public bool RemoveByName(string name)
        {
            foreach (Connection connection in this)
            {
                if (connection.Name == name)
                {
                    this.Remove(connection);
                    return true;
                }
            }
            return false;
        }
    }
}
