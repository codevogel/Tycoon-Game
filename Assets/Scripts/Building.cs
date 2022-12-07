using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Placeable
{
    public Resource[] UpkeepStorage;
    public Resource[] ProductionStorage;
    private BuildingPreset LocalPreset { get; set; }

    public Building(BuildingPreset preset)
    {
        Preset = preset;
        LocalPreset = preset;
        UpkeepStorage = new Resource[preset.Upkeep.Length];
        ProductionStorage = new Resource[preset.Production.Length];
    }

    public bool Build()
    {
        //if (ResourceManager.Instance.CheckEnoughResources(Preset.BuildCost))
        //{
        //    ResourceManager.Instance.RemoveResource(Preset.BuildCost);
        //    ResourceManager.Instance.AddResource(Preset.InitialProduction);
        //    return true;
        //}
        return false;
    }

    public bool DoUpkeep()
    {
        if (Resource.CheckEnoughResources(UpkeepStorage, LocalPreset.Upkeep))
        {
            Resource.RemoveResource(UpkeepStorage, LocalPreset.Upkeep);
        }
        return false;
    }

    public void Produce()
    {
        Resource.AddResource(ProductionStorage, LocalPreset.Production);
    }
}
