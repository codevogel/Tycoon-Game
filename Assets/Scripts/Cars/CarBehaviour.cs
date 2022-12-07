using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Cars
{
    public class CarBehaviour : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        [SerializeField] private Transform target;

        [SerializeField] private bool pathActive;

        // Start is called before the first frame update
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (pathActive)
            {
                _navMeshAgent.SetDestination(target.position);
            }
        }
        /// <summary>
        /// Set AreaMask for Agent
        /// </summary>
        /// <param name="area"></param>
        [Button]
        private void SetArea(int area)
        {
            _navMeshAgent.areaMask = area;
            //Beware: areaMask uses binary
        }
    }
}