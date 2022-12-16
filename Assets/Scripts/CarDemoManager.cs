using System.Collections.Generic;
using NavMesh;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class CarDemoManager : MonoBehaviour
{
    [SerializeField] private bool pathActive;

    private List<AgentBehaviour> _cars;
    private List<NavMeshAgent> _agents;

    public Transform cil1, cil2;

    // Start is called before the first frame update
    private void Start()
    {
        _cars = new List<AgentBehaviour>();
        foreach (var car in GetComponentsInChildren<AgentBehaviour>())
        {
            _cars.Add(car);
        }

        _agents = new List<NavMeshAgent>();
        foreach (var agent in GetComponentsInChildren<NavMeshAgent>())
        {
            _agents.Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pathActive)
        {
            foreach (var car in _cars)
            {
                car.pathActive = true;
            }
        }
    }

    [Button]
    private void ChangeAgentTag()
    {
        foreach (var agent in _agents)
        {
            agent.areaMask = agent.areaMask switch
            {
                9 => 17,
                17 => 9,
                _ => agent.areaMask
            };
        }
    }

    [Button]
    private void ChangeCilPos()
    {
        var cil1Base = cil1.position;
        var cil2Base = cil2.position;

        cil1.position = cil2Base;
        cil2.position = cil1Base;
    }
}