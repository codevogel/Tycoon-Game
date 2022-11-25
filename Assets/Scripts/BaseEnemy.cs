using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public int health = 100;

    [SerializeField] private Transform target;

    private NavMeshAgent _navMeshAgent;

    [SerializeField] private bool activateTarget;

    // Start is called before the first frame update
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (activateTarget)
        {
            _navMeshAgent.isStopped = false;
            SetTarget(target);
        }
        else
        {
            _navMeshAgent.isStopped = true;
        }
    }

    private void SetTarget(Transform targetObj)
    {
        _navMeshAgent.SetDestination(targetObj.position);
    }
}