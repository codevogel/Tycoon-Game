using UnityEngine;
using UnityEngine.AI;

namespace Cars
{
    public class CarBehaviour : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        [SerializeField] private Transform target;

        // Start is called before the first frame update
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
          //.  _navMeshAgent.areaMask
          //TODO: implement area mask (layer) for cars
        }

        // Update is called once per frame
        void Update()
        {
            _navMeshAgent.SetDestination(target.position);
        }
    }
}