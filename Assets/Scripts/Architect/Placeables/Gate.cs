using Agency;
using Architect.Placeables.Presets;
using Buildings;
using Grid;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Architect.Placeables
{
    /// <summary>
    /// Defines behaviour for the gates.
    /// Extends from Building.
    /// </summary>
    public class Gate : Building
    {
        TargetBehaviour targetBehaviour;
        public int RepairAmount = 3;
        public Gate(BuildingPreset preset) : base(preset)
        {

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
            targetBehaviour = Tile.PlaceableHolder.GetComponentInChildren<TargetBehaviour>();
        }

        /// <summary>
        /// Handles the BuildingController Produce hook.
        /// </summary>
        protected override void Fabricate()
        {
            if (targetBehaviour.health > targetBehaviour.BaseHealth - RepairAmount) return;
            base.Fabricate();
            targetBehaviour.health += RepairAmount;
        }
    }
}
