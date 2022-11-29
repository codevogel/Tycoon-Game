using System;

public enum ResourceType
{
    People,
    AvailablePeople,
    Food,
    Minerals,
    Ammo
}

[Serializable]
public class Resource
{
    public ResourceType Type;
    public int Amount;
    private static int typeCount = -1;
    public Resource(ResourceType type, int amount = 0)
    {
        Type = type;
        Amount = amount;
    }

    public static int TypeCount
    {
        get
        {
            if (typeCount == -1) typeCount = Enum.GetValues(typeof(ResourceType)).Length;
            return typeCount;
        }
    }

    //Logic for resourceArrays
    /// <summary> Adds the resource to the resourceArray.</summary>
    public static void AddResource(Resource[] resourceCollectionToAddTo, Resource resourceToAdd)
    {
        for (int i = 0; i < resourceCollectionToAddTo.Length; i++)
        {
            if (resourceCollectionToAddTo[i].Type == resourceToAdd.Type)
            {
                resourceCollectionToAddTo[i].Amount += resourceToAdd.Amount;
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
            if (resourceCollectionToRemoveFrom[i].Type == resourceToRemove.Type)
            {
                resourceCollectionToRemoveFrom[i].Amount -= resourceToRemove.Amount;
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
            if (resourceCollectionToCheck[i].Type == resourceToCheck.Type)
            {
                return resourceCollectionToCheck[i].Amount >= resourceToCheck.Amount;
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
