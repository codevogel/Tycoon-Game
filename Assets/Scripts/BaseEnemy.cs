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
            SetTarget(target);
        }
    }

    private void SetTarget(Transform target)
    {
        _navMeshAgent.SetDestination(target.position);
    }
}