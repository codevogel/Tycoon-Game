using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agency
{
    public class BaseEnemy : AgentBehaviour
    {
        public int health;
        public int damage;
        public int speed;

        private Vector3 _startPos;
        private float _timer;

        [SerializeField] private List<GameObject> attackTargets = new();

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            SetTimer();
            OnDeath();
            GoToTarget();
            CheckForInactive();
        }

        #region logic

        private void CheckForInactive() //checks for inactive object in attackTargets list
        {
            foreach (var target in attackTargets.Where(target => !target.activeSelf))
            {
                attackTargets.Remove(target);
                return;
            }
        }

        private void OnDeath()
        {
            if (health > 0) return;
            transform.localPosition = _startPos;
            OnReleaseAgent();
            health = GameManager.Instance.enemyBaseHealth;
        }

        private void GoToTarget()
        {
            var target = GetTarget();

            if (target == null) return;

            // Move our position a step closer to the target.
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

        private void SetTimer()
        {
            _timer += Time.deltaTime;
        }

        private GameObject GetTarget()
        {
            GameObject closestTarget = null;
            float mDist = Mathf.Infinity;

            if (attackTargets == null) return null;

            foreach (var targetInRange in attackTargets)
            {
                if (closestTarget == null) closestTarget = targetInRange;

                var distance = Vector3.Distance(targetInRange.transform.position, transform.position);
                if (!(distance < mDist)) continue;

                mDist = distance;
                closestTarget = targetInRange;
            }

            return closestTarget;
        }

        #endregion

        #region Collisions and Triggers

        private void OnCollisionStay(Collision collision)
        {
            CheckForInactive();
            if (!collision.collider.CompareTag("Gate")) return;
            if (!collision.collider.TryGetComponent(out TargetBehaviour targetBehaviour)) return;
            if (!(_timer > targetBehaviour.armor)) return;
            if (targetBehaviour.health > 0)
            {
                targetBehaviour.DoDamage(damage);
                _timer = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Gate")) return;
            if (!attackTargets.Contains(other.gameObject)) attackTargets.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            attackTargets.Remove(other.gameObject);
        }

        #endregion
    }
}