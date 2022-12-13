using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

namespace RoadBehviour
{
    public class RoadNav : MonoBehaviour
    {
        /// <summary>
        /// Build NavMesh-grid
        /// </summary>
        [Button]
        private void BuildNavSingle()
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}