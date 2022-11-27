using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public int health = 100;

    [SerializeField] private Transform target;
    [SerializeField] private bool activateTarget;

    private NavMeshAgent _navMeshAgent;
    private Action<BaseEnemy> _killAction;

    private Vector3 _startPos;

    // Start is called before the first frame update
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _startPos = transform.localPosition;
    }

    private void Update()
    {
        target = EnemySpawner.Instance.currentTarget;
        if (activateTarget)
        {
            _navMeshAgent.isStopped = false;
            SetTarget(target);
        }
        else
        {
            _navMeshAgent.isStopped = true;
        }

        CheckHealth();
    }


    private void CheckHealth()
    {
        if (health <= 0)
        {
            EnemySpawner.Instance.KillEnemy(gameObject);
            transform.localPosition = _startPos;
        }
    }

    private void SetTarget(Transform targetObj)
    {
        _navMeshAgent.SetDestination(targetObj.position);
    }
}