using System;
using System.Collections.Generic;
using System.Linq;
using Agency;
using Buildings.Resources;
using Grid;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Architect.Placeables
{
    public class BaseTower : MonoBehaviour
    {
        [BoxGroup("ammo")] [SerializeField] private TextMeshPro ammoText;

        [BoxGroup("Shooting Behaviour")] [SerializeField]
        private int damage = 10;

        [BoxGroup("Shooting Behaviour")] [SerializeField]
        private float cooldown = 1f;

        private List<GameObject> _enemyList = new();
        private ParticleSystem _bulletParticleSys;
        private Building _building;
        private float _timer;
        private GameObject currentTarget;


        private void Awake()
        {
            _bulletParticleSys = GetComponentInChildren<ParticleSystem>();
            _building = transform.parent.parent.parent.GetComponent<Tile>()
                .Content as Building; //TODO make this less jank
        }

        private void Update()
        {
            SetTimer();
            CheckInactive();
            SetText();
            EnemyInTrigger();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Enemy"))
            {
                DoDamage(other);
            }
        }

        /// <summary>
        /// add enemies to the detected list
        /// </summary>
        /// <param name="enemy">enemy to add to list</param>
        public void AddEnemyToList(GameObject enemy)
        {
            _enemyList.Add(enemy);
        }

        /// <summary>
        /// remove enemies from the detected list
        /// </summary>
        /// <param name="enemy">enemy to remove to list</param>
        public void RemoveEnemyFromList(GameObject enemy)
        {
            _enemyList.Remove(enemy);
        }

        /// <summary>
        /// behaviour when an enemy enters/stays in trigger radius
        /// </summary>
        public void EnemyInTrigger()
        {
            if (_enemyList.Count <= 0) return;
            if (NewTarget() == null) return;
            transform.LookAt(NewTarget().transform); //barrel looks at detected enemy
            Debug.Log("Target: " + NewTarget());
            if (!(_timer > cooldown) || _building.Input.Get(ResourceType.Ammo) <= 0) return;
            _timer = 0;
            _building.Input.Remove(new Resource(ResourceType.Ammo, 1)); //ammo depletes based on ammo efficiency
            _bulletParticleSys.Play(); //play/shoot bullets/particles
        }

        /// <summary>
        /// Do damage to an enemy gameObject
        /// </summary>
        /// <param name="obj">enemy gameObject</param>
        private void DoDamage(GameObject obj)
        {
            if (obj.TryGetComponent(out BaseEnemy enemy))
            {
                enemy.health -= damage;
            }
        }

        /// <summary>
        /// Check if gameObject in list is inactive
        /// </summary>
        private void CheckInactive()
        {
            foreach (var enemy in _enemyList.ToArray().Where(enemy => !enemy.activeSelf))
            {
                _enemyList.Remove(enemy); //remove gameObject from list if inactive
            }
        }

        /// <summary>
        /// sets a new (enemy) target based on distance to tower
        /// </summary>
        /// <returns></returns>
        public GameObject NewTarget()
        {
            currentTarget = _enemyList[0];
            var distanceToCurrentTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            foreach (var t in _enemyList)
            {
                var distanceToEnemy = Vector3.Distance(transform.position, t.transform.position);

                if (distanceToEnemy < distanceToCurrentTarget)
                {
                    currentTarget = t;
                }
            }

            return currentTarget;
        }

        private void SetTimer()
        {
            _timer += Time.deltaTime;
        }

        private void SetText()
        {
            ammoText.text = _building.Input.Get(ResourceType.Ammo).ToString();
        }
    }
}