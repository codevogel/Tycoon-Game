using Architect.Placeables.Presets;
using Buildings;
using Grid;
using UI;
using UnityEngine;

namespace Architect.Placeables
{
    /// <summary>
    /// Defines behaviour for the construction site for a building.
    /// Extends from Building.
    /// </summary>
    public class ConstructionSite : Building
    {
        public BuildingPreset PresetToConstruct;
        public bool isReceiving = false;

        public ConstructionSite(BuildingPreset preset, BuildingPreset toConstruct) : base(preset)
        {
            PresetToConstruct = toConstruct;
            ProductionCost = toConstruct.buildCost;
        }

        /// <summary>
        /// Initializes a buildings fields after it has been instantiated.
        /// </summary>
        /// <param name="hostingTile">The tile this building is hosted on.</param>
        public override void InitializeAfterInstantiation(Tile hostingTile)
        {
            Tile = hostingTile;
            BuildingController.Instance.SubscribeBuilding(this, true, false);
            BuildingConnectionsRenderer = Tile.transform.Find("Recipient Lines").GetComponent<BuildingConnectionsRenderer>();
        }

        /// <summary>
        /// Handler for the produce hook.
        /// </summary>
        protected override void Fabricate()
        {
            bool HasRequiredResources = Input.HasResourcesRequired(ProductionCost);
            OnFabricate?.Invoke(HasRequiredResources);
            if (!HasRequiredResources)
            {
                return;
            }
            Tile.RemoveContent();
            ArchitectController.Instance.PlaceBuildingAt(Tile, PresetToConstruct);
        }
    }
}
