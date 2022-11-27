using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public Transform currentTarget;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int spawnCount;

    private ObjectPool<GameObject> _enemyPool;

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
        // Update is called once per frameRespawn()
    }

    private void Update()
    {
        Respawn();
    }

    private void CreateEnemyPool()
    {
        _enemyPool = new ObjectPool<GameObject>(
            () => Instantiate(enemyPrefab, transform),
            enemy => { enemy.SetActive(true); },
            enemy => { enemy.SetActive(false); },
            Destroy, true, 10, 20);
    }

    private void Spawn()
    {
        for (var i = 0; i < spawnCount; i++)
        {
            var enemy = _enemyPool.Get();
            enemy.transform.localPosition = new Vector3(i * 2, 1, 0);
        }
    }

    private void Respawn()
    {
        if (_enemyPool.CountActive <= 5)
        {
            _enemyPool.Get();
        }
    }

    public void KillEnemy(GameObject obj)
    {
        _enemyPool.Release(obj);
        obj.transform.localPosition = transform.localPosition;
    }
}