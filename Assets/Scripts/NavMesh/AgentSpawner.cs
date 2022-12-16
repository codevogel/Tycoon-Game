using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace NavMesh
{
    public class AgentSpawner : MonoBehaviour
    {
        public List<GameObject> agentList;

        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private Transform agentTarget;
        [SerializeField] private int spawnCount;

        private ObjectPool<GameObject> _agentPool;

        /// <summary>
        /// return agent to pool
        /// </summary>
        /// <param name="agent">agent to return to pool</param>
        public void ReleaseAgent(GameObject agent)
        {
            _agentPool.Release(agent);
        }

        private void SetAgentTargets(GameObject agent, Transform target)
        {
            GetAgentData(agent).targetList.Add(target != null ? target : GameManager.Instance.globalTarget.transform);
        }

        private void Start()
        {
            CreateAgentPool();
            SpawnMultiple(spawnCount);
        }

        /// <summary>
        /// Create agent objectPool
        /// </summary>
        private void CreateAgentPool()
        {
            _agentPool = new ObjectPool<GameObject>(
                InstantiateAgent,
                OnGet,
                OnRelease,
                Destroy, true, 10, 20);
        }

        /// <summary>
        /// retrieves the AgentBehaviour class of a given agent
        /// </summary>
        /// <param name="agent">agent to retrieve the behaviour class of</param>
        /// <returns></returns>
        private AgentBehaviour GetAgentData(GameObject agent)
        {
            if (agentList != null)
            {
                if (agent.TryGetComponent(out AgentBehaviour agentBehaviour))
                {
                    return agentBehaviour;
                }
            }

            Debug.Log($"{agentList} list is empty");
            return null;
        }

        /// <summary>
        /// instantiate
        /// </summary>
        /// <returns></returns>
        private GameObject InstantiateAgent()
        {
            var o = Instantiate(agentPrefab, transform);
            if (agentList.Contains(o)) return o;
            agentList.Add(o);
            GetAgentData(o).spawnOrigin = this;
        //    SetAgentTargets(o, agentTarget);
            return o;
        }


        /// <summary>
        /// collection method for OnActionGet in objectPool
        /// </summary>
        /// <param name="agent">agent to perform object pool get function on</param>
        private void OnGet(GameObject agent)
        {
            agent.SetActive(true);
            SetAgentTargets(agent, agentTarget);
            //   if (!agentList.Contains(agent)) return;
            //  agentList.Add(agent);
        }

        /// <summary>
        /// collection method for OnActionRelease in objectPool
        /// </summary>
        /// <param name="agent">agent to perform object pool release function on</param>
        private static void OnRelease(GameObject agent)
        {
            agent.SetActive(false);
        }

        /// <summary>
        /// Spawn multiple enemies
        /// </summary>
        /// <param name="spawnAmount">amount of agents to spawn</param>
        [Button("spawn multiple")]
        private void SpawnMultiple(int spawnAmount)
        {
            for (var i = 0; i < spawnAmount; i++)
            {
                var agent = _agentPool.Get();
                agent.transform.localPosition = new Vector3(i * 2, 0, 0);
            }
        }
    }
}