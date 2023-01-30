using Architect.Placeables;
using Buildings.Resources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Agency
{
    [RequireComponent(typeof(AgentTargetRenderer))]
    public class DeliveryAgent : AgentBehaviour
    {
        public Resource[] storage;
        public UnityEvent<bool> OnMoving;

        private AgentTargetRenderer targetRenderer;
        private int _standStill;

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

        private void OnTriggerStay(Collider other)
        {
            if (TargetList.Count > 0 && other == TargetList[0].Entrance.TileCollider)
            {
                //Check if agent has reached destination or if the collider is from the building tile
                if ((_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                    _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete) ||
                    TargetList[0].Entrance == TargetList[0].Tile)
                {
                    TargetList[0].AddToStorage(TargetList[0].Input, storage);
                    RemoveTarget();
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            targetRenderer.SetOriginAndTarget(spawnOrigin.transform, TargetList[0].Tile.transform);
            if (TargetList[0].Tile.Content == null) RemoveTarget();

            if (_navMeshAgent.velocity.magnitude > 1f)
            {
                _standStill = 0;
            }
            OnMoving.Invoke(_standStill++ > 10);
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