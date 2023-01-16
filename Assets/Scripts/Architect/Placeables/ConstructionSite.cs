using Architect.Placeables.Presets;
using Grid;
using UI;
using UnityEngine;

namespace Architect.Placeables
{
    public class ConstructionSite : Building
    {

        public BuildingPreset PresetToConstruct;

        public ConstructionSite(BuildingPreset preset, BuildingPreset toConstruct) : base(preset)
        {
            PresetToConstruct = toConstruct;
            ProductionCost = toConstruct.buildCost;
        }

        //TODO: better Building hierarchy to reduce duplicate code
        public override void InitializeAfterInstantiation(Tile hostingTile)
        {
            Tile = hostingTile;
            Buildings.BuildingController.SubscribeBuilding(this, true, false);
            BuildingConnectionsRenderer = Tile.transform.Find("Recipient Lines").GetComponent<BuildingConnectionsRenderer>();
        }

        protected override void Fabricate()
        {
            if (!ArchitectController.Instance.FirstBuilding && !Input.HasResourcesRequired(ProductionCost))
            {
                Debug.Log("Could not fabricate in Construction Site");
                return;
            }
            Tile.RemoveContent();
            ArchitectController.Instance.PlaceBuildingAt(Tile, PresetToConstruct);
        }
    }
}
