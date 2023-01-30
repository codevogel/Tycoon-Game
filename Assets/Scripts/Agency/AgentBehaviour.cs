using System.Collections.Generic;
using Architect.Placeables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Agency
{
    /// <summary>
    /// Handles all NavMesh behaviour of agents
    /// is base class for NavMeshAgents.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentBehaviour : MonoBehaviour
    {
        public AgentSpawner spawnOrigin;
        public List<Building> TargetList = new();

        protected NavMeshAgent _navMeshAgent;

        [ReadOnly] public bool onMesh;

        protected void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        [Button]
        private void ResetPath()
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.SetDestination(TargetList[0].Tile.PlaceableHolder.position);
        }

        protected virtual void FixedUpdate()
        {
            SetTarget();
            onMesh = _navMeshAgent.isOnNavMesh;
        }

        protected virtual void SetTarget()
        {
            if (TargetList.Count <= 0 || _navMeshAgent.hasPath || !_navMeshAgent.isOnNavMesh) return;
            _navMeshAgent.SetDestination(TargetList[0].Tile.PlaceableHolder.position);
        }

        protected void RemoveTarget()
        {
            TargetList.RemoveAt(0);
            if (TargetList.Count <= 0) OnReleaseAgent();
        }

        /// <summary>
        /// activates spawner objectPool OnRelease()
        /// </summary>
        protected void OnReleaseAgent()
        {
            TargetList.Clear();
            transform.position = spawnOrigin.transform.position;
            spawnOrigin.ReleaseAgent(this);
        }
    }
}