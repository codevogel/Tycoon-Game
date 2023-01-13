using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace NavMesh
{
    public class AgentSpawner : MonoBehaviour
    {
        public List<AgentBehaviour> agentList;

        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private Building agentTarget;
        [SerializeField] private int spawnCount;
        [SerializeField] private int spawnMax;

        [SerializeField] private bool useIntervals;

        [ShowIfGroup("useIntervals")] [SerializeField]
        private float interval;

        private float _spawnTimer;

        [SerializeField] private bool useDelay = false;

        [ShowIfGroup("useDelay")] [SerializeField]
        private float delayTimer = 60;

        [SerializeField] private TextMeshPro delaytimerText;

        private ObjectPool<AgentBehaviour> _agentPool;


        /// <summary>
        /// return agent to pool
        /// </summary>
        /// <param name="agent">agent to return to pool</param>
        public void ReleaseAgent(AgentBehaviour agent)
        {
            _agentPool.Release(agent);
        }

        private void SetAgentTargets(AgentBehaviour agent, Building target)
        {
            //agent.targetList.Add(target != null ? target : GameManager.Instance.globalTarget.transform);
            agent.targetList.Add(target);
        }

        private void Start()
        {
            CreateAgentPool();
        }

        private void Update()
        {
            if (useDelay)
            {
                SpawnDelay();
            }
            else
            {
                StartCoroutine(SpawnMultiple(spawnCount));
                _spawnTimer += Time.deltaTime;

                if (_spawnTimer > interval && useIntervals)
                {
                    _spawnTimer = 0;
                    SpawnMultipleAgents();
                }
            }
        }

        /// <summary>
        /// Create agent objectPool
        /// </summary>
        private void CreateAgentPool()
        {
            _agentPool = new ObjectPool<AgentBehaviour>(
                InstantiateAgent,
                OnGet,
                OnRelease,
                Destroy, true, spawnMax, spawnMax);
        }

        ///// <summary>
        ///// retrieves the AgentBehaviour class of a given agent
        ///// </summary>
        ///// <param name="agent">agent to retrieve the behaviour class of</param>
        ///// <returns></returns>
        //private AgentBehaviour GetAgentData(AgentBehaviour agent)
        //{
        //    if (agentList != null)
        //    {
        //        if (agent.TryGetComponent(out AgentBehaviour agentBehaviour))
        //        {
        //            return agentBehaviour;
        //        }
        //    }

        //    Debug.Log($"{agentList} list is empty");
        //    return null;
        //}

        /// <summary>
        /// instantiate
        /// </summary>
        /// <returns></returns>
        private AgentBehaviour InstantiateAgent()
        {
            if (_agentPool.CountAll < spawnMax)
            {
                AgentBehaviour agent = Instantiate(agentPrefab, transform).GetComponent<AgentBehaviour>();
                if (agentList.Contains(agent)) return agent;
                agentList.Add(agent);
                agent.spawnOrigin = this;
                return agent;
            }

            Debug.LogWarning($"max spawncount reached on {this}");
            return null;
        }

        /// <summary>
        /// collection method for OnActionGet in objectPool
        /// </summary>
        /// <param name="agent">agent to perform object pool get function on</param>
        private void OnGet(AgentBehaviour agent)
        {
            agent.gameObject.SetActive(true);
            //SetAgentTargets(agent, agentTarget);
            //   if (!agentList.Contains(agent)) return;
            //  agentList.Add(agent);
        }

        /// <summary>
        /// collection method for OnActionRelease in objectPool
        /// </summary>
        /// <param name="agent">agent to perform object pool release function on</param>
        private static void OnRelease(AgentBehaviour agent)
        {
            agent.gameObject.SetActive(false);
        }

        public AgentBehaviour SpawnAgent()
        {
            if (_agentPool.CountActive < spawnMax) return _agentPool.Get();
            return null;
        }

        [Button("spawn multiple")]
        private void SpawnMultipleAgents()
        {
            StartCoroutine(SpawnMultiple(spawnCount));
        }

        /// <summary>
        /// sets a delay before agents are able to spawn
        /// </summary>
        private void SpawnDelay()
        {
            delayTimer -= Time.deltaTime;

            var timeTable = TimeSpan.FromSeconds(delayTimer);
            delaytimerText.text = $"{timeTable.Minutes:D2}:{timeTable.Seconds:D2}";
            if (delayTimer <= 0)
            {
                useDelay = false;
            }
        }

        /// <summary>
        /// Spawn multiple enemies
        /// </summary>
        /// <param name="spawnAmount">amount of agents to spawn</param>
        private IEnumerator SpawnMultiple(int spawnAmount)
        {
            if (_agentPool.CountAll < spawnMax)
            {
                for (var i = 0; i < spawnAmount; i++)
                {
                    var agent = _agentPool.Get();
                    agent.transform.localPosition = new Vector3(0, 0, 0);
                    yield return new WaitForSeconds(.5f);
                }
            }


            yield return new WaitForEndOfFrame();
        }
    }
}