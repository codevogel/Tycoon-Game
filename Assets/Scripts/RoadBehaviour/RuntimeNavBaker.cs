using System;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

namespace RoadBehaviour
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class RuntimeNavBaker : MonoBehaviour
    {
        public static RuntimeNavBaker Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// Build NavMesh-grid
        /// </summary>
        [Button]
        public void BakeNavMesh()
        {
            if (TryGetComponent(out NavMeshSurface nms))
            {
                nms.BuildNavMesh();
            }
        }
    }
}