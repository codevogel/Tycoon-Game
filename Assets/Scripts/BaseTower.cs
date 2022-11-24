using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.UIElements;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
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

    void Update()
    {
        BarrelBehaviour();
        setTimer();
    }

    /// <summary>
    /// Barrel Aiming Behaviour
    /// </summary>
    public void BarrelBehaviour()
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
        if (enemyList.Contains(other.gameObject))
        {
            if (_timer > cooldown)
            {
                enemyList[0].GetComponent<BaseEnemy>().health -= damage;
                _timer = 0;
                _ps.Play();
            }
        }
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

    private void setTimer()
    {
        _timer += Time.deltaTime;
    }
}