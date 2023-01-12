using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavMesh
{
    public class DeliveryAgent : AgentBehaviour
    {
        public Resource[] storage;

        public void SetDeliveryTarget(Resource[] toDeliver, Building target)
        {
            storage = toDeliver;
            targetList.Add(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (targetList.Count > 0 && other == targetList[0].Tile.TileCollider)
            {
                //Debug.Log("Found Target!");
                targetList[0].AddToStorage(targetList[0].input, storage);
                targetList.RemoveAt(0);
                if (targetList.Count <= 0) OnReleaseAgent();
            }
        }
    }
}