using System;
using System.Dynamic;
using UnityEngine;
using System.Reflection;
using Object = UnityEngine.Object;

namespace HOEngine
{
    /// <summary>
    /// 单例类 必须包含私有构造 防止外部创建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singlton<T> where T:class
    {
        private static T mInstance;

        private static readonly object LockObj = new object();

        public static T Instacne()
        {
            if (mInstance == null)
            {
                lock (LockObj)
                {
                    mInstance = (T)Activator.CreateInstance(typeof(T),true);
                }
            }
            return mInstance;
        }
    }

    /// <summary>
    /// Mono 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
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