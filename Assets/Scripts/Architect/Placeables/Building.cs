using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : Placeable
{

    public enum BuildingType
    {
        Factory,
        Storage,
        Tower
    }

    public Storage input;
    public Storage output;
    public int productionTime;

    public Resource[] productionCost;
    public Resource[] produces;

    private BuildingPreset LocalPreset { get; set; }

    /// <summary>
    /// Creates a new building with a given BuildingPreset.
    /// </summary>
    /// <param name="preset">The preset for this building.</param>
    public Building(BuildingPreset preset)
    {
        Preset = preset;
        LocalPreset = preset;
        input = new Storage(preset.InitialStorage);
        output = new Storage(Array.Empty<Resource>());
        productionCost = LocalPreset.ProductionCost;
        produces = LocalPreset.Produces;

        SubscribeToBuildingController();
    }

    /// <summary>
    /// Subscribes to building controller cycles.
    /// </summary>
    private void SubscribeToBuildingController()
    {
        switch (LocalPreset.buildingType)
        {
            case BuildingType.Factory:
                BuildingController.Produce.AddListener(Produce);
                break;
            case BuildingType.Storage:
                break;
            case BuildingType.Tower:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Production cycle for this building.
    /// </summary>
    private void Produce()
    {
        if (BuildingController.Tick % productionTime == 0)
        {
            Fabricate();
        }
    }

    /// <summary>
    /// Fabricates the resources from productionCost into the resources from produces.
    /// </summary>
    private void Fabricate()
    {
        if (input.HasResourcesRequired(productionCost))
            return;
        foreach (Resource resource in productionCost)
        {
            input.Remove(resource);
        }
        foreach (Resource resource in produces)
        {
            output.Add(resource);
        }
    }


    /// <summary>
    /// A storage holds a collection of resources.
    /// </summary>
    [Serializable]
    public class Storage
    {
        public Dictionary<ResourceType, int> Contents { get; set; } = new();

        /// <summary>
        /// Creates a storage, and adds the contents of initialStorage to it.
        /// </summary>
        /// <param name="initialStorage">The initial resources in this storage.</param>
        public Storage(Resource[] initialStorage)
        {
            foreach (Resource resource in initialStorage)
            {
                Add(resource);
            }
        }

        /// <summary>
        /// Does the storage contain the required resources for this production cycle?
        /// </summary>
        /// <param name="required">The required resources for this production cycle.</param>
        /// <returns>Whether enough resources are contained in the storage.</returns>
        public bool HasResourcesRequired(Resource[] required)
        {
            foreach (Resource resource in required)
            {
                if (Contents[resource.Type] - resource.Amount < 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a resource item to the storage
        /// </summary>
        /// <param name="resource">Adds a resource to the storage.</param>
        public void Add(Resource resource) { Contents[resource.Type] += resource.Amount;}

        /// <summary>
        /// Removes a resource item from the storage.
        /// </summary>
        /// <param name="resource">Removes a resource to the storage.</param>
        public void Remove(Resource resource) { Contents[resource.Type] -= resource.Amount; }
    }
}
