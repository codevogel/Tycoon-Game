using System.Collections.Generic;
using System.Linq;
using NavMesh;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Towers
{
    public class BaseTower : MonoBehaviour
    {
        [BoxGroup("ammo")] [ReadOnly] [SerializeField]
        private float ammo = 100;

        [BoxGroup("ammo")] [SerializeField] private int baseAmmo = 100;
        [BoxGroup("ammo")] [SerializeField] private TextMeshPro ammoText;

        [BoxGroup("ammo")] [SerializeField] [Range(.75f, 1)]
        private float efficiencyMultiplier;

        [BoxGroup("Shooting Behaviour")] [SerializeField]
        private int damage = 10;

        [BoxGroup("Shooting Behaviour")] [SerializeField]
        private float cooldown = 1f;

        private List<GameObject> _enemyList = new();
        private ParticleSystem _bulletParticleSys;
        private float _timer;


        private void Awake()
        {
            _bulletParticleSys = GetComponentInChildren<ParticleSystem>();
            ammo = baseAmmo;
        }

        private void Update()
        {
            SetTimer();
            CheckInactive();
            SetText();
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
        /// behaviour when an enemy enters/stays in radius
        /// </summary>
        public void EnemyInTrigger()
        {
            if (_enemyList.Count <= 0) return;
            transform.LookAt(_enemyList?[0].transform); //barrel looks at detected enemy
            if (!(_timer > cooldown) || ammo <= 0) return;
            _timer = 0;
            ammo -= efficiencyMultiplier; //ammo depletes based on ammo efficiency
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

        private void SetTimer()
        {
            _timer += Time.deltaTime;
        }

        private void SetText()
        {
            ammoText.text = Mathf.RoundToInt(ammo).ToString();
        }
    }
}