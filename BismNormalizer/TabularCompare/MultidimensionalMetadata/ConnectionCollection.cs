using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BismNormalizer.TabularCompare.MultidimensionalMetadata
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
        /// Find an object in the collection by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Connection object if found. Null if not found.</returns>
        public Connection FindById(string id)
        {
            foreach (Connection connection in this)
            {
                if (connection.Id == id)
                {
                    return connection;
                }
            }
            return null;
        }

        /// <summary>
        /// A Boolean specifying whether the collection contains object by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the object is found, or False if it's not found.</returns>
        public bool ContainsId(string id)
        {
            foreach (Connection connection in this)
            {
                if (connection.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes an object from the collection by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the object was removed, or False if was not found.</returns>
        public bool RemoveById(string id)
        {
            foreach (Connection connection in this)
            {
                if (connection.Id == id)
                {
                    this.Remove(connection);
                    return true;
                }
            }
            return false;
        }
    }
}
