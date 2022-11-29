using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Enemies
{
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
            SpawnMultiple(spawnCount);
        }

        /// <summary>
        /// return gameObject to pool
        /// </summary>
        /// <param name="obj">gameObject to return to pool</param>
        public void KillEnemy(GameObject obj)
        {
            _enemyPool.Release(obj);
        }

        /// <summary>
        /// Create enemy objectpool
        /// </summary>
        private void CreateEnemyPool()
        {
            _enemyPool = new ObjectPool<GameObject>(
                () => Instantiate(enemyPrefab, transform),
                enemy => { enemy.SetActive(true); },
                enemy => { enemy.SetActive(false); },
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
                var enemy = _enemyPool.Get();
                enemy.transform.localPosition = new Vector3(i * 2, 1, 0);
            }
        }

        /// <summary>
        /// spawn a single enemy
        /// </summary>
        [Button("spawn enemy")]
        private void SpawnSingle()
        {
            _enemyPool.Get();
        }
    }
}