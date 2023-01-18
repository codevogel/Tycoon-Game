using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Agency
{
    public class AgentSpawner : MonoBehaviour
    {
        public List<AgentBehaviour> agentList;
        public int waveCounter;

        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private int spawnCount;

        [Tooltip("the amount of enemies that get added to the next wave")] [SerializeField]
        private int spawnScalar;

        [SerializeField] private int spawnMax;
        private Building _agentTarget;

        [SerializeField] private bool useIntervals;

        [ShowIfGroup("useIntervals")] [SerializeField]
        private float interval;

        private float _spawnTimer;

        [SerializeField] private bool useDelay;

        [ShowIfGroup("useDelay")] [ReadOnly] [SerializeField]
        private float delayTimer = 60;

        [ShowIfGroup("useDelay")] [SerializeField]
        private float delayAmount = 60;

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
            agent.TargetList.Add(target);
        }

        private void Start()
        {
            CreateAgentPool();
            delayTimer = delayAmount;
        }

        private void Update()
        {
            if (useDelay)
            {
                if (!SpawnDelay())
                {
                    StartCoroutine(SpawnMultiple(spawnCount));
                    _spawnTimer += Time.deltaTime;

                    // if (_spawnTimer > interval && useIntervals)
                    // {
                    //     _spawnTimer = 0;
                    //     SpawnMultipleAgents();
                    // }
                }
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
            Debug.Log(transform.name);
            _agentPool = new ObjectPool<AgentBehaviour>(
                InstantiateAgent,
                OnGet,
                OnRelease,
                Destroy, true, spawnMax, spawnMax);
        }

        /// <summary>
        /// instantiate agents for the objectPool;
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
        public void SpawnMultipleAgents()
        {
            StartCoroutine(SpawnMultiple(spawnCount));
        }

        /// <summary>
        /// sets a delay before agents are able to spawn
        /// </summary>
        private bool SpawnDelay()
        {
            delayTimer -= Time.deltaTime;

            var timeTable = TimeSpan.FromSeconds(delayTimer);
            delaytimerText.text = $"{timeTable.Minutes:D2}:{timeTable.Seconds:D2}";
            if (delayTimer <= 0)
            {
                delayTimer = delayAmount;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Spawn multiple enemies
        /// </summary>
        /// <param name="spawnAmount">amount of agents to spawn</param>
        private IEnumerator SpawnMultiple(int spawnAmount)
        {
            if (_agentPool.CountAll < spawnMax)
            {
                waveCounter++;
                for (var i = 0; i < spawnAmount; i++)
                {
                    var agent = _agentPool.Get();
                    agent.transform.localPosition = new Vector3(0, 0, 0);
                    yield return new WaitForSeconds(.5f);
                }

                spawnCount += spawnScalar;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}