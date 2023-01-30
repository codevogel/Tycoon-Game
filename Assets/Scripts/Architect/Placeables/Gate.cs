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
    public class Gate : Building
    {
        TargetBehaviour targetBehaviour;
        public int RepairAmount = 3;
        public Gate(BuildingPreset preset) : base(preset)
        {

        }

        //TODO: better Building hierarchy to reduce duplicate code
        public override void InitializeAfterInstantiation(Tile hostingTile)
        {
            Tile = hostingTile;
            BuildingController.Instance.SubscribeBuilding(this, true, false);
            BuildingConnectionsRenderer = Tile.transform.Find("Recipient Lines").GetComponent<BuildingConnectionsRenderer>();
            targetBehaviour = Tile.PlaceableHolder.GetComponentInChildren<TargetBehaviour>();
        }

        protected override void Fabricate()
        {
            if (targetBehaviour.health > targetBehaviour.BaseHealth - RepairAmount) return;
            base.Fabricate();
            targetBehaviour.health += RepairAmount;
        }
    }
}
