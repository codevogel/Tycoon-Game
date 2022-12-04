using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public BuildingPreset Preset;
    public Resource[] UpkeepStorage;
    public Resource[] ProductionStorage;

    public Building(BuildingPreset preset)
    {
        Preset = preset;
        UpkeepStorage = new Resource[preset.Upkeep.Length];
        ProductionStorage = new Resource[preset.Production.Length];
    }

    public bool Build()
    {
        if (ResourceManager.Instance.CheckEnoughResources(Preset.BuildCost))
        {
            ResourceManager.Instance.RemoveResource(Preset.BuildCost);
            ResourceManager.Instance.AddResource(Preset.InitialProduction);
            return true;
        }
        return false;
    }

    public bool DoUpkeep()
    {
        if (Resource.CheckEnoughResources(UpkeepStorage, Preset.Upkeep))
        {
            Resource.RemoveResource(UpkeepStorage, Preset.Upkeep);
        }
        return false;
    }

    public void Produce()
    {
        Resource.AddResource(ProductionStorage, Preset.Production);
    }
}
