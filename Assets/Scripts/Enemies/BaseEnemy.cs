using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour
    {
        public int health;
        public int damage;

        [SerializeField] private Transform target;
        [SerializeField] private bool activateTarget;

        private NavMeshAgent _navMeshAgent;
        private Vector3 _startPos;
        private float _timer;

        // Start is called before the first frame update
        private void Awake()
        {
            health = GameManager.Instance.enemyBaseHealth;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _startPos = transform.localPosition;
        }


        private void Update()
        {
            SetTimer();
            target = GameManager.Instance.currentEnemyTarget.transform;
            if (activateTarget)
            {
                _navMeshAgent.isStopped = false;
                SetTarget(target);
            }
            else
            {
                _navMeshAgent.isStopped = true;
            }

            OnDeath();
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!collision.collider.CompareTag("Target")) return;
            if (!collision.collider.TryGetComponent(out TargetBehaviour targetBehaviour)) return;
            if (!(_timer > targetBehaviour.armor)) return;

            targetBehaviour.DoDamage(damage);
            _timer = 0;
        }

        private void SetTimer()
        {
            _timer += Time.deltaTime;
        }

        private void OnDeath()
        {
            if (health > 0) return;
            transform.localPosition = _startPos;
            EnemySpawner.Instance.KillEnemy(gameObject);
            health = GameManager.Instance.enemyBaseHealth;
        }

        private void SetTarget(Transform targetObj)
        {
            _navMeshAgent.SetDestination(targetObj.position); //sets navmesh target
        }
    }
}