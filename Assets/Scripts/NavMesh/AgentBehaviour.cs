using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace NavMesh
{
    public class AgentBehaviour : MonoBehaviour
    {
        public AgentSpawner spawnOrigin;
        private static NavMeshAgent NavMeshAgent { get; set; }

        public List<Transform> targetList;

        public bool pathActive;


        // Start is called before the first frame update
        protected virtual void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            SetTarget();
            Debug.Log(NavMeshAgent.hasPath);
        }

        [Button]
        private void SetTarget()
        {
            if (targetList.Count <= 0) return;
            //  if (!pathActive) return;
            NavMeshAgent.SetDestination(targetList[0].position);
            Debug.Log("Target set");
        }

        /// <summary>
        /// activates spawner objectPool OnRelease()
        /// </summary>
        protected virtual void OnReleaseAgent()
        {
            spawnOrigin.ReleaseAgent(gameObject);
        }
    }
}