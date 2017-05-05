using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Represents a collection of MDependency objects.
    /// </summary>
    public class MDependencyCollection : List<MDependency>
    {
        /// <summary>
        /// Find an object in the collection by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>MDependency object if found. Null if not found.</returns>
        public MDependency FindByName(string name)
        {
            foreach (MDependency mDependency in this)
            {
                if (mDependency.ObjectName == name)
                {
                    return mDependency;
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
            foreach (MDependency mDependency in this)
            {
                if (mDependency.ObjectName == name)
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
        public bool Remove(string name)
        {
            foreach (MDependency mDependency in this)
            {
                if (mDependency.ObjectName == name)
                {
                    this.Remove(mDependency);
                    return true;
                }
            }
            return false;
        }
    }
}
