using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

namespace RoadBehaviour
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class RuntimeNavBaker : MonoBehaviour
    {
        /// <summary>
        /// Build NavMesh-grid
        /// </summary>
        [Button]
        private void BakeNavMesh()
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}