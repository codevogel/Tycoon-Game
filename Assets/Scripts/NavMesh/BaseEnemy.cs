using UnityEngine;

namespace NavMesh
{
    public class BaseEnemy : AgentBehaviour
    {
        public int health;
        public int damage;

        private Vector3 _startPos;
        private float _timer;

        private void Awake()
        {
            health = GameManager.Instance.enemyBaseHealth;
        }

        protected override void Update()
        {
            base.Update();
            SetTimer();
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

        private void OnDeath()
        {
            if (health > 0) return;
            transform.localPosition = _startPos;
            OnReleaseAgent();
            health = GameManager.Instance.enemyBaseHealth;
        }

        private void SetTimer()
        {
            _timer += Time.deltaTime;
        }
    }
}