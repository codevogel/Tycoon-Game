using Sirenix.OdinInspector;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    public ResourceUI[] uiObjects;
    [TableList, SerializeField] private Resource[] resources;

    /// <summary> Adds the resource to resources. If the resource is the same as People it also adds AvailablePeople</summary>
    public void AddResource(Resource resource)
    {
        resources[(int)resource.Type].Amount += resource.Amount;
        if (resource.Type == ResourceType.People) resources[(int)ResourceType.AvailablePeople].Amount += resource.Amount;
    }

    /// <summary> Adds the resources to this.resources. If the resource is the same as People it also adds AvailablePeople</summary>
    public void AddResource(Resource[] resources)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            AddResource(resources[i]);
        }
    }

    /// <summary> Removes the resource from resources. If the resource is the same as People it also removes AvailablePeople</summary>
    public void RemoveResource(Resource resource)
    {
        resources[(int)resource.Type].Amount -= resource.Amount;
        if (resource.Type == ResourceType.People) resources[(int)ResourceType.AvailablePeople].Amount -= resource.Amount;
    }

    /// <summary> Removes the resource to resources. If the resource is the same as People it also adds AvailablePeople</summary>
    public void RemoveResource(Resource[] resources)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            RemoveResource(resources[i]);
        }
    }

    /// <summary>Checks to see if there are more or equal resources to the amount of resources that is given</summary>
    /// <returns>True if there are enough resources, false if there are not</returns>
    public bool CheckEnoughResources(Resource resource)
    {
        return Resource.CheckEnoughResources(resources, resource);
    }

    /// <summary>Checks to see if there are more or equal resources to the amount of resources that is given</summary>
    /// <returns>True if there are enough resources, false if there are not</returns>
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