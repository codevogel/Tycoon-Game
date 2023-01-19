using Buildings.Resources;
using System;
using UnityEngine;

namespace Agency
{
    [RequireComponent(typeof(AgentTargetRenderer))]
    public class DeliveryAgent : AgentBehaviour
    {
        public Resource[] storage;

        private AgentTargetRenderer targetRenderer;

        private void Awake()
        {
            targetRenderer = GetComponent<AgentTargetRenderer>();    
        }

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
            if (TargetList.Count > 0 && other == TargetList[0].Entrance.TileCollider)
            {
                //Debug.Log("Found Target!");
                TargetList[0].AddToStorage(TargetList[0].Input, storage);
                TargetList.RemoveAt(0);
                if (TargetList.Count <= 0) OnReleaseAgent();
            }
        }

        protected override void Update()
        {
            base.Update();
            targetRenderer.SetOriginAndTarget(spawnOrigin.transform, TargetList[0].Tile.transform);
        }

        internal void OnSelect()
        {
            targetRenderer.SetOriginAndTarget(spawnOrigin.transform, TargetList[0].Tile.transform);
            targetRenderer.ShowLines(true);
        }

        internal void OnDeselect()
        {
            targetRenderer.ShowLines(false);
        }

        protected override void SetTarget()
        {
            if (TargetList.Count <= 0 || _navMeshAgent.hasPath || !_navMeshAgent.isOnNavMesh) return;
            _navMeshAgent.SetDestination(TargetList[0].Entrance.PlaceableHolder.position);
        }
    }
}