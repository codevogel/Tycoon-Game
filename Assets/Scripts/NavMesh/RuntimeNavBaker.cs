using System.Collections;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

namespace NavMesh
{
    /// <summary>
    /// Build NavMesh in runtime based on a NavMeshSurface component
    /// </summary>
    [RequireComponent(typeof(NavMeshSurface))]
    public class RuntimeNavBaker : MonoBehaviour
    {
        public static RuntimeNavBaker Instance;

        private void Awake()
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
        /// Instantly Build NavMesh-grid
        /// </summary>
        [Button]
        public void InstantBake()
        {
            if (TryGetComponent(out NavMeshSurface nms))
            {
                nms.BuildNavMesh();
            }
        }

        /// <summary>
        /// bake navmesh on FixedUpdate()
        /// </summary>
        /// <returns></returns>
        public void DelayedBake()
        {
            StartCoroutine(FixedBake());
        }

        /// <summary>
        /// Timed routine to bake navmesh on FixedUpdate()
        /// </summary>
        /// <returns></returns>
        private IEnumerator FixedBake()
        {
            yield return new WaitForFixedUpdate();
            if (TryGetComponent(out NavMeshSurface nms))
            {
                nms.BuildNavMesh();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}