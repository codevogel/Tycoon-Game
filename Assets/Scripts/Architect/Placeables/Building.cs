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

    private void Produce()
    {
        if (BuildingController.Tick % productionTime == 0)
        {
            Fabricate();
        }
    }
    private void Fabricate()
    {
        if (input.HasResourcesRequired(productionCost))
            return;
        foreach (Resource resource in productionCost)
        {
            input.RemoveItem(resource);
            output.AddItem(resource);
        }
    }

    [Serializable]
    public class Storage
    {
        public Dictionary<ResourceType, int> Contents { get; set; } = new();

        public Storage(Resource[] initialStorage)
        {
            foreach (Resource resource in initialStorage)
            {
                AddItem(resource);
            }
        }

        public bool HasResourcesRequired(Resource[] required)
        {
            foreach (Resource resource in required)
            {
                if (Contents[resource.Type] - resource.Amount < 0)
                    return false;
            }
            return true;
        }

        public void AddItem(Resource resource) { Contents[resource.Type] += resource.Amount;}

        public void RemoveItem(Resource resource) { Contents[resource.Type] -= resource.Amount; }
    }
}
