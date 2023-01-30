using System;

namespace Buildings.Resources
{
    /// <summary>
    /// Enum for types of resources that exist.
    /// </summary>
    public enum ResourceType
    {
        Iron,
        Ammo
    }

    /// <summary>
    /// Base class for a Resource.
    /// </summary>
    [Serializable]
    public class Resource
    {
        public ResourceType type;
        public int amount;
        private static int _typeCount = -1;
        public Resource(ResourceType type, int amount = 0)
        {
            this.type = type;
            this.amount = amount;
        }

        /// <summary>Returns the amount of different resources</summary>
        public static int TypeCount
        {
            get
            {
                if (_typeCount == -1) _typeCount = Enum.GetValues(typeof(ResourceType)).Length;
                return _typeCount;
            }
        }

        //Logic for resourceArrays
        /// <summary> Adds the resource to the resourceArray.</summary>
        public static void AddResource(Resource[] resourceCollectionToAddTo, Resource resourceToAdd)
        {
            for (int i = 0; i < resourceCollectionToAddTo.Length; i++)
            {
                if (resourceCollectionToAddTo[i].type == resourceToAdd.type)
                {
                    resourceCollectionToAddTo[i].amount += resourceToAdd.amount;
                    return;
                }
            }
        }

        /// <summary> Adds the resources to the resourceArray.</summary>
        public static void AddResource(Resource[] resourceCollectionToAddTo, Resource[] resourcesToAdd)
        {
            for (int i = 0; i < resourcesToAdd.Length; i++)
            {
                AddResource(resourceCollectionToAddTo, resourcesToAdd[i]);
            }
        }

        /// <summary> Removes the resource from the resourceArray.</summary>
        public static void RemoveResource(Resource[] resourceCollectionToRemoveFrom, Resource resourceToRemove)
        {
            for (int i = 0; i < resourceCollectionToRemoveFrom.Length; i++)
            {
                if (resourceCollectionToRemoveFrom[i].type == resourceToRemove.type)
                {
                    resourceCollectionToRemoveFrom[i].amount -= resourceToRemove.amount;
                    return;
                }
            }
        }

        /// <summary> Removes the resources from the resourceArray.</summary>
        public static void RemoveResource(Resource[] resourceCollectionToRemoveFrom, Resource[] resourcesToRemove)
        {
            for (int i = 0; i < resourcesToRemove.Length; i++)
            {
                RemoveResource(resourceCollectionToRemoveFrom, resourcesToRemove[i]);
            }
        }

        /// <summary>Checks to see if there are more or equal resources to the amount of resources that is given</summary>
        /// <returns>True if there are enough resources, false if there are not</returns>
        public static bool CheckEnoughResources(Resource[] resourceCollectionToCheck, Resource resourceToCheck)
        {
            for (int i = 0; i < resourceCollectionToCheck.Length; i++)
            {
                if (resourceCollectionToCheck[i].type == resourceToCheck.type)
                {
                    return resourceCollectionToCheck[i].amount >= resourceToCheck.amount;
                }
            }
            return false;
        }

        /// <summary>Checks to see if there are more or equal resources to the amount of resources that is given</summary>
        /// <returns>True if there are enough resources</returns>
        public static bool CheckEnoughResources(Resource[] resourceCollectionToCheck, Resource[] resourcesToCheck)
        {
            for (int i = 0; i < resourcesToCheck.Length; i++)
            {
                if (!CheckEnoughResources(resourceCollectionToCheck, resourcesToCheck[i])) return false;
            }
            return true;
        }
    }
}