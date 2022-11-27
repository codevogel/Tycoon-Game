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
    public static void AddResource(Resource[] resourceCollectionToAddTo, Resource resourceToAdd)
    {
        resourceCollectionToAddTo[(int)resourceToAdd.Type].Amount += resourceToAdd.Amount;
        if (resourceToAdd.Type == ResourceType.People) resourceCollectionToAddTo[(int)ResourceType.AvailablePeople].Amount += resourceToAdd.Amount;
    }

    public static void AddResource(Resource[] resourceCollectionToAddTo, Resource[] resourcesToAdd)
    {
        for (int i = 0; i < resourcesToAdd.Length; i++)
        {
            AddResource(resourceCollectionToAddTo, resourcesToAdd[i]);
        }
    }

    public static void RemoveResource(Resource[] resourceCollectionToRemoveFrom, Resource resourceToRemove)
    {
        resourceCollectionToRemoveFrom[(int)resourceToRemove.Type].Amount -= resourceToRemove.Amount;
        if (resourceToRemove.Type == ResourceType.People) resourceCollectionToRemoveFrom[(int)ResourceType.AvailablePeople].Amount -= resourceToRemove.Amount;
    }

    public static void RemoveResource(Resource[] resourceCollectionToRemoveFrom, Resource[] resourcesToRemove)
    {
        for (int i = 0; i < resourcesToRemove.Length; i++)
        {
            RemoveResource(resourceCollectionToRemoveFrom, resourcesToRemove[i]);
        }
    }

    public static bool CheckEnoughResources(Resource[] resourceCollectionToCheck, Resource resourceToCheck)
    {
        return resourceCollectionToCheck[(int)resourceToCheck.Type].Amount >= resourceToCheck.Amount;
    }

    public static bool CheckEnoughResources(Resource[] resourceCollectionToCheck, Resource[] resourcesToCheck)
    {
        for (int i = 0; i < resourcesToCheck.Length; i++)
        {
            if (!CheckEnoughResources(resourceCollectionToCheck, resourcesToCheck[i])) return false;
        }
        return true;
    }
}
