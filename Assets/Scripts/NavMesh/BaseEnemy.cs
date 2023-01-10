using System;
using System.Collections.Generic;
using UnityEngine;

namespace NavMesh
{
    public class BaseEnemy : AgentBehaviour
    {
        public int health;
        public int damage;
        public int speed;

        private Vector3 _startPos;
        [SerializeField]private float _timer;

        [SerializeField] private List<GameObject> targets = new List<GameObject>();

        protected override void Update()
        {
            base.Update();
            SetTimer();
            OnDeath();
            
            var target = GetTarget();

            // Move our position a step closer to the target.
            var step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

        private void OnCollisionStay(Collision collision)
        {
            Debug.Log("Collision");
            if (!collision.collider.CompareTag("Walls")) return;
            Debug.Log("Wall exist");
            if (!collision.collider.TryGetComponent(out TargetBehaviour targetBehaviour)) return;
            Debug.Log("Script is real");
            if (!(_timer > targetBehaviour.armor)) return;

            targetBehaviour.DoDamage(damage);
            Debug.Log(targetBehaviour);
            _timer = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Walls")) return;
            if (!targets.Contains(other.gameObject)) targets.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            targets.Remove(other.gameObject);
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

        private GameObject GetTarget()
        {
            GameObject closestTarget = null;
            float mDist = Mathf.Infinity; 
            
            if (targets == null) return null;
            
            foreach (var VARIABLE in targets)
            {
                if (closestTarget == null) closestTarget = VARIABLE;
                
                var distance = Vector3.Distance(VARIABLE.transform.position, transform.position);
                if (!(distance < mDist)) continue;
                
                mDist = distance;
                closestTarget = VARIABLE;
            }
            
            return closestTarget;
        }
    }
}