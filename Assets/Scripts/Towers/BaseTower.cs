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
        [BoxGroup("ammo")] public int ammo = 100;

        [BoxGroup("ammo")] [SerializeField] private TextMeshPro ammoText;

        //  [SerializeField] private GameObject barrel;
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
            SetText();
        }

        /// <summary>
        /// Do damage to an enemy gameObject
        /// </summary>
        /// <param name="obj">enemy gameObject</param>
        public void DoDamage(GameObject obj)
        {
            if (obj.TryGetComponent(out BaseEnemy enemy))
            {
                enemy.health -= damage;
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Enemy"))
            {
                DoDamage(other);
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
                transform.LookAt(enemyList?[0].transform);
            }
        }

        public void AddEnemyToList(GameObject enemy)
        {
            enemyList.Add(enemy);
        }

        public void RemoveEnemyFromList(GameObject enemy)
        {
            enemyList.Remove(enemy);
        }

        public void EnemyInTrigger(GameObject enemy)
        {
            if (enemyList.Count <= 0) return;
            if (!(_timer > cooldown) || ammo <= 0) return;
            _timer = 0;
            ammo--;
            _ps.Play();
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

        private void SetText()
        {
            ammoText.text = ammo.ToString();
        }
    }
}