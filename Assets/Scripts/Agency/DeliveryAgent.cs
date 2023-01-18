using Buildings.Resources;
using UnityEngine;

namespace Agency
{
    public class DeliveryAgent : AgentBehaviour
    {
        public Resource[] storage;

        public void SetDeliveryTarget(Resource[] toDeliver, Building target)
        {
            storage = toDeliver;
            AddTarget(target);
        }

        public void AddTarget(Building target)
        {
            TargetList.Add(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TargetList.Count > 0 && other == TargetList[0].Tile.TileCollider)
            {
                //Debug.Log("Found Target!");
                TargetList[0].AddToStorage(TargetList[0].Input, storage);
                TargetList.RemoveAt(0);
                if (TargetList.Count <= 0) OnReleaseAgent();
            }
        }
    }
}