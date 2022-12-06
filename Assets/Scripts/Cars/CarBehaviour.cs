using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Cars
{
    public class CarBehaviour : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        [SerializeField] private Transform target;

        [SerializeField] private string areaIndex;

        // Start is called before the first frame update
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();


            //TODO: implement area mask (layer) for cars
        }

        // Update is called once per frame
        void Update()
        {
            _navMeshAgent.SetDestination(target.position);
        }

        [Button]
        private void SetArea(string area)
        {
            _navMeshAgent.areaMask = Convert.ToInt32(area)+1;
            var stuff = Convert.ToByte(NavMesh.GetAreaFromName(area));
            Debug.Log(stuff);
                //Oneway = 8
            //OneWay, Walkable = 9
        }
    }
}