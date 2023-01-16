using UnityEngine;

namespace Utils
{
    [DefaultExecutionOrder(-100)]
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = typeof(T).ToString();
                    instance = gameObject.AddComponent<T>();
                    //  DontDestroyOnLoad(gameObject);
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance == this as T)
            {
                //  DontDestroyOnLoad(gameObject);
                return;
            }

            if (instance == null)
            {
                instance = this as T;
                //   DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning($"Singleton of type {typeof(T)} Already exists. Destroying object attached to it.");
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (instance != this)
                return;

            instance = null;
        }
    }
}