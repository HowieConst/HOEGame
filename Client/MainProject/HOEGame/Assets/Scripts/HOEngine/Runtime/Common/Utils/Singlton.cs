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
            lock (LockObj)
            {
                return mInstance ??= (T)Activator.CreateInstance(typeof(T), true);
            }
        }
    }

    /// <summary>
    /// Mono 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSinglton<T> : MonoBehaviour where T:MonoBehaviour
    {
        private static T mInstance;
        private static readonly object LockObj = new object();
        public static T Instance()
        {
            
            if (applicationIsQuitting) {  
                Debug.LogWarning("[Singleton] Instance '"+ typeof(T) +  
                                 "' already destroyed on application quit." +  
                                 " Won't create again - returning null.");  
                return null;  
            }

            lock (LockObj)
            {
                if (mInstance != null) return mInstance;
                mInstance = (T)FindObjectOfType(typeof(T));
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +  
                                   " - there should never be more than 1 singleton!" +  
                                   " Reopenning the scene might fix it.");  
                    return mInstance; 
                }
                if(mInstance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<T>();
                }
                return mInstance;
            }
     
        }
        private static bool applicationIsQuitting = false;  
   
        public void OnDestroy () {  
            applicationIsQuitting = true;  
        } 
        
    }
}