using Sirenix.OdinInspector;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    public ResourceUI[] uiObjects;
    [TableList, SerializeField] private Resource[] resources;

    public void AddResource(Resource resource)
    {
        resources[(int)resource.Type].Amount += resource.Amount;
        if (resource.Type == ResourceType.People) resources[(int)ResourceType.AvailablePeople].Amount += resource.Amount;
    }

    public void AddResource(Resource[] resources)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            AddResource(resources[i]);
        }
    }

    public void RemoveResource(Resource resource)
    {
        resources[(int)resource.Type].Amount -= resource.Amount;
        if (resource.Type == ResourceType.People) resources[(int)ResourceType.AvailablePeople].Amount -= resource.Amount;
    }

    public void RemoveResource(Resource[] resources)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            RemoveResource(resources[i]);
        }
    }

    public bool CheckEnoughResources(Resource resource)
    {
        return Resource.CheckEnoughResources(resources, resource);
    }

    public bool CheckEnoughResources(Resource[] resources)
    {
        return Resource.CheckEnoughResources(this.resources, resources);
    }

    [Button]
    public void ResetResources()
    {
        uiObjects = UsefullFunctions.FindAssetsByType<ResourceUI>().ToArray();
        resources = new Resource[Resource.TypeCount];
        for (int i = 0; i < Resource.TypeCount; i++)
        {
            resources[i] = new Resource((ResourceType)i);
        }
    }
}