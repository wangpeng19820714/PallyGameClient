using UnityEngine;

namespace GameFramework
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T mInstance = null;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (mInstance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        mInstance = go.AddComponent<T>();
                        mInstance.enabled = true;
                        GameObject parent = GameObject.Find("Boot");
                        if (parent == null)
                        {
                            parent = new GameObject("Boot");
                            if (Application.isPlaying)
                            {
                                GameObject.DontDestroyOnLoad(parent);
                            }
                        }
                        if (parent != null)
                        {
                            go.transform.parent = parent.transform;
                        }
                    }
                }

                return mInstance;
            }
        }
        public void Startup()
        {

        }

        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as T;
            }

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
            Init();
        }

        protected virtual void Init()
        {

        }
        public void DestroySelf()
        {
            Dispose();
            MonoSingleton<T>.mInstance = null;
            UnityEngine.Object.Destroy(gameObject);
        }

        public virtual void Dispose()
        {
        }

        public virtual long CacheSize()
        {
            return 0;
        }

        public virtual void ClearCache()
        {

        }
    }
}
