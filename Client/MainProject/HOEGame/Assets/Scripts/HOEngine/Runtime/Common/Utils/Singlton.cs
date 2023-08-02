using UnityEngine;
namespace HOEngine
{
    public class Singlton<T> where T:new()
    {
        private static T mInstance;

        public static T Instacne()
        {
            if (mInstance == null)
            {
                mInstance = new T();
            }

            return mInstance;
        }
    }

    public class MonoSinglton<T> where T:MonoBehaviour
    {
        public static T Instance()
        {
            var instances = (GameObject[])Object.FindObjectsOfType(typeof(T));
            if (instances.Length > 0)
            {
                if (instances.Length > 1)
                {
                    Debug.LogWarning("find multiple instance");
                    for (int i = 1; i < instances.Length; i++)
                    {
                        Object.DestroyImmediate(instances[i]);
                    }
                }
                return instances[0].GetComponent<T>();
            }
            else
            {
                var go = new GameObject(typeof(T).Name);
                Object.DontDestroyOnLoad(go);
                var t = go.AddComponent<T>();
                return t;
            }
        }
    }
}