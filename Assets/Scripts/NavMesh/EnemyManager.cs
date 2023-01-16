using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavMesh
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<AgentSpawner> enemySpawnerList;
        [SerializeField] private GameObject startButton;


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
    }
}