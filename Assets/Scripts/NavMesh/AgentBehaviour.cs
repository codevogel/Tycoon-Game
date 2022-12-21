using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace NavMesh
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentBehaviour : MonoBehaviour
    {
        public AgentSpawner spawnOrigin;
        public List<Building> targetList = new List<Building>();
        public Resource[] storage;

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
            _navMeshAgent.SetDestination(targetList[0].Tile.Root.position);
        }

        protected virtual void Update()
        {
            SetTarget();
            onMesh = _navMeshAgent.isOnNavMesh;
        }

        private void SetTarget()
        {
            if (targetList.Count <= 0 || _navMeshAgent.hasPath || !_navMeshAgent.isOnNavMesh) return;
            _navMeshAgent.SetDestination(targetList[0].Tile.Root.position);
        }

        /// <summary>
        /// activates spawner objectPool OnRelease()
        /// </summary>
        protected void OnReleaseAgent()
        {
            targetList.Clear();
            spawnOrigin.ReleaseAgent(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == targetList[0].Tile.TileCollider)
            {
                targetList[0].AddToStorage(targetList[0].input, storage);
            }
        }
    }
}