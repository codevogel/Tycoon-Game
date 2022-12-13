using System;
using System.Collections;
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
            DontDestroyOnLoad(this);
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
        public void InstantBake()
        {
            if (TryGetComponent(out NavMeshSurface nms))
            {
                nms.BuildNavMesh();
            }
        }

        public void DelayedBake()
        {
            StartCoroutine(FixedBake());
        }

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