using System.Collections.Generic;
using Cars;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Enemies
{
    public class AgentSpawner : MonoBehaviour
    {
        public static AgentSpawner Instance;

        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private int spawnCount;

        private ObjectPool<GameObject> _agentPool;
        [SerializeField] private Transform agentTarget;
        public List<GameObject> _AgentList;

        [SerializeField] private bool activateAgents;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            setAgentTarget();
        }

        private void setAgentTarget()
        {
            if (_AgentList != null)
            {
                Debug.Log("doing stuff");
                foreach (var t in _AgentList)
                {
                    if (t.TryGetComponent(out CarBehaviour nme))
                    {
                        nme.target = agentTarget;
                    }

                    nme.pathActive = activateAgents;
                }
            }
        }


        private void Start()
        {
            CreateAgentPool();
            SpawnMultiple(spawnCount);
        }

        /// <summary>
        /// return gameObject to pool
        /// </summary>
        /// <param name="obj">gameObject to return to pool</param>
        public void KillAgent(GameObject obj)
        {
            _agentPool.Release(obj);
        }

        /// <summary>
        /// Create agent objectpool
        /// </summary>
        private void CreateAgentPool()
        {
            _agentPool = new ObjectPool<GameObject>(
                InstantiateObject,
                OnGet,
                OnRelease,
                Destroy, true, 10, 20);
        }

        /// <summary>
        /// Spawn multiple enemies
        /// </summary>
        /// <param name="spawnAmount">amount to spawn</param>
        private void SpawnMultiple(int spawnAmount)
        {
            for (var i = 0; i < spawnAmount; i++)
            {
                var agent = _agentPool.Get();
                agent.transform.localPosition = new Vector3(i * 2, 1, 0);
            }
        }

        private GameObject InstantiateObject()
        {
            var o = Instantiate(agentPrefab, transform);
            if (_AgentList.Contains(o)) return o;
            _AgentList.Add(o);
            return o;
        }

        private void OnGet(GameObject agent)
        {
            agent.SetActive(true);
            if (!_AgentList.Contains(agent)) return;
            _AgentList.Add(agent);
        }

        private void OnRelease(GameObject agent)
        {
            agent.SetActive(false);
        }

        /// <summary>
        /// spawn a single agent
        /// </summary>
        [Button("spawn agent")]
        private void SpawnSingle()
        {
            _agentPool.Get();
        }
    }
}