using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Towers
{
    public class BaseTower : MonoBehaviour
    {
        public int ammo = 100;
        [SerializeField] private TextMeshPro ammoText;
        [SerializeField] private GameObject barrel;
        [SerializeField] private int damage = 10;
        [SerializeField] private float cooldown = 1f;
        [ReadOnly] [SerializeField] private List<GameObject> enemyList = new();
        private ParticleSystem _ps;
        private float _timer;


        private void Awake()
        {
            _ps = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            BarrelBehaviour();
            SetTimer();
            CheckInactive();
            setText();
        }

        /// <summary>
        /// Do damage to an enemy gameObject
        /// </summary>
        /// <param name="obj">enemy gameObject</param>
        public void DoDamage(GameObject obj)
        {
            if (enemyList.Contains(obj))
            {
                obj.GetComponent<BaseEnemy>().health -= damage;
            }
        }

        /// <summary>
        /// Barrel Aiming Behaviour
        /// </summary>
        private void BarrelBehaviour()
        {
            if (enemyList.Count > 0)
            {
                //aim at first enemy in list
                barrel.transform.LookAt(enemyList?[0].transform);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            if (enemyList.Contains(other.gameObject) == false)
            {
                //add Enemy to list when colliding
                enemyList.Add(other.gameObject);
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (!enemyList.Contains(other.gameObject)) return;
            if (!(_timer > cooldown) && ammo > 0) return;
            _timer = 0;
            ammo--;
            _ps.Play();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            if (enemyList.Contains(other.gameObject))
            {
                //remove Enemy from list when exiting collider
                enemyList.Remove(other.gameObject);
            }
        }

        /// <summary>
        /// Check if gameobject in list is inactive
        /// </summary>
        private void CheckInactive()
        {
            foreach (var enemy in enemyList.ToArray().Where(enemy => !enemy.activeSelf))
            {
                enemyList.Remove(enemy);
            }
        }

        private void SetTimer()
        {
            _timer += Time.deltaTime;
        }

        private void setText()
        {
            ammoText.text = ammo.ToString();
        }
    }
}