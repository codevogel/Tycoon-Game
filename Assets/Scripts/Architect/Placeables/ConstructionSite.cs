using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSite : Building
{

    private BuildingPreset presetToConstruct;

    public ConstructionSite(BuildingPreset preset, BuildingPreset toConstruct) : base(preset)
    {
        presetToConstruct = toConstruct;
        productionCost = toConstruct.BuildCost;
    }

    //TODO: better Building hierarchy to reduce duplicate code
    public override void InitializeAfterInstantiation(Tile hostingTile)
    {
        Tile = hostingTile;
        BuildingController.SubscribeBuilding(this, true, false);
        BuildingConnectionsRenderer = Tile.transform.Find("Recipient Lines").GetComponent<BuildingConnectionsRenderer>();
    }

    protected override void Fabricate()
    {
        if (!ArchitectController.FirstBuilding && !input.HasResourcesRequired(productionCost))
        {
            Debug.Log("Could not fabricate in Construction Site");
            return;
        }
        Tile.RemoveContent();
        ArchitectController.Instance.PlaceBuildingAt(Tile, presetToConstruct);
    }
}
