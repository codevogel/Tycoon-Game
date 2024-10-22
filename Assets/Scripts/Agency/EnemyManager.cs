using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Agency
{
    /// <summary>
    /// Handles the enemy spawners.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<AgentSpawner> enemySpawnerList;
        [SerializeField] private GameObject startButton;
        [SerializeField] [ReadOnly] private int waveCounter;

        [Tooltip(
            "how many waves it takes for a new spawner to get activated, set to 0 for spawners to activate instantly")]
        [SerializeField]
        private int waveThreshold;

        private void Awake()
        {
            if (waveThreshold <= 0)
            {
                Debug.Log($"waveThreshold on {this} = 0, all spawners will activate at the same time");
            }
        }

        private void Update()
        {
            waveCounter = enemySpawnerList[0].waveCounter;
            AddSpawner();
        }

        public void StartWaveSpawn()
        {
            StartCoroutine(SpawnWave());
        }

        public IEnumerator SpawnWave()
        {
            enemySpawnerList[0].gameObject.SetActive(true);
            startButton.SetActive(false);
            yield return new WaitForFixedUpdate();
            enemySpawnerList[0].SpawnMultipleAgents();
            yield return new WaitForEndOfFrame();
        }

        private void AddSpawner()
        {
            for (int i = 1; i < enemySpawnerList.Count; i++)
            {
                if (waveCounter == waveThreshold * (i))
                {
                    enemySpawnerList[i].gameObject.SetActive(true);
                }
            }
        }
    }
}