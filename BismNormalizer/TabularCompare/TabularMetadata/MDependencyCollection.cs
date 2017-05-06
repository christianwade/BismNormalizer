using System;
using System.Collections.Generic;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Represents a collection of MDependency objects.
    /// </summary>
    public class MDependencyCollection : List<MDependency>
    {
        /// <summary>
        /// Returns collection of M dependencies that the object identified by the params references (directly or indirectly).
        /// </summary>
        /// <param name="objectType">Type of the object to look up dependencies.</param>
        /// <param name="objectName">Name of the object to look up dependencies.</param>
        /// <returns></returns>
        public MDependencyCollection DependenciesReferenceFrom(MDependencyObjectType objectType, string objectName)
        {
            MDependencyCollection returnVal = new MDependencyCollection();
            LookUpDependenciesReferenceFrom(objectType, objectName, returnVal);
            return returnVal;
        }

        private void LookUpDependenciesReferenceFrom(MDependencyObjectType objectType, string objectName, MDependencyCollection returnVal)
        {
            foreach (MDependency mDependency in this)
            {
                if (mDependency.ObjectType == objectType && mDependency.ObjectName == objectName)
                {
                    LookUpDependenciesReferenceFrom(mDependency.ReferencedObjectType, mDependency.ReferencedObjectName, returnVal);
                    returnVal.Add(mDependency);
                }
            }
        }

        /// <summary>
        /// Returns collection of M dependency objects that hold references to the object identified by the param values (directly or chained).
        /// </summary>
        /// <param name="objectType">Type of the object to look up dependencies.</param>
        /// <param name="objectName">Name of the object to look up dependencies.</param>
        /// <returns></returns>
        public MDependencyCollection DependenciesReferenceTo(MDependencyObjectType referencedObjectType, string referencedObjectName)
        {
            MDependencyCollection returnVal = new MDependencyCollection();
            LookUpDependenciesReferenceTo(referencedObjectType, referencedObjectName, returnVal);
            return returnVal;
        }

        private void LookUpDependenciesReferenceTo(MDependencyObjectType referencedObjectType, string referencedObjectName, MDependencyCollection returnVal)
        {
            foreach (MDependency mDependency in this)
            {
                if (mDependency.ReferencedObjectType == referencedObjectType && mDependency.ReferencedObjectName == referencedObjectName)
                {
                    LookUpDependenciesReferenceTo(mDependency.ObjectType, mDependency.ObjectName, returnVal);
                    returnVal.Add(mDependency);
                }
            }
        }

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
