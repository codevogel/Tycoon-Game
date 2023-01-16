using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Agency
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentBehaviour : MonoBehaviour
    {
        public AgentSpawner spawnOrigin;
        public List<Building> TargetList = new();

        private NavMeshAgent _navMeshAgent;

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

        protected virtual void Update()
        {
            SetTarget();
            onMesh = _navMeshAgent.isOnNavMesh;
        }

        private void SetTarget()
        {
            if (TargetList.Count <= 0 || _navMeshAgent.hasPath || !_navMeshAgent.isOnNavMesh) return;
            _navMeshAgent.SetDestination(TargetList[0].Tile.PlaceableHolder.position);
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