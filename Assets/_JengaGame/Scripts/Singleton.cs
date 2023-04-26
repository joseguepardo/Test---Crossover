using UnityEngine;

namespace JengaGame
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var obj = new GameObject
                        {
                            name = typeof(T).Name
                        };
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }

            private set => _instance = value;
        }

        protected virtual void Awake()
        {
            CreateSingleton();
        }

        private void CreateSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}