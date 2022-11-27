using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public Transform currentTarget;

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int spawnCount;

    private ObjectPool<GameObject> _enemyPool;

    public static int BaseHealth = 100;

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

    private void Start()
    {
        CreateEnemyPool();
        Spawn();
    }


    private void CreateEnemyPool()
    {
        _enemyPool = new ObjectPool<GameObject>(
            () => Instantiate(enemyPrefab, transform),
            enemy => { enemy.SetActive(true); },
            enemy => { enemy.SetActive(false); },
            Destroy, true, 10, 20);
    }

    [Button("spawn dude")]
    private void Spawn()
    {
        for (var i = 0; i < spawnCount; i++)
        {
            var enemy = _enemyPool.Get();
            enemy.transform.localPosition = new Vector3(i * 2, 1, 0);
        }
    }

    [Button("spawn enemy")]
    private void Respawn()
    {
        _enemyPool.Get();
    }

    public void KillEnemy(GameObject obj)
    {
        _enemyPool.Release(obj);
        
        obj.transform.localPosition = transform.localPosition;
    }
}