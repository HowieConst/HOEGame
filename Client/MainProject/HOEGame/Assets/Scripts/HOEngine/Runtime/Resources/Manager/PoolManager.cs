using System.Collections.Generic;
using UnityEngine;

namespace HOEngine.Resources
{
    internal sealed class PoolManager :Singlton<PoolManager>,IEngineManager
    {
        private PoolManager()
        {
            
        }

        private Dictionary<string, PoolObject> PoolObjectsMap;
        private Transform poolRootTrans;

        internal PoolObject LoadPoolObject(string name)
        {
            if (PoolObjectsMap.TryGetValue(name, out var poolObject))
            {
                return poolObject;
            }
            return null;
        }

        internal PoolObject CreatePoolObject(string name)
        {
            var poolObject = ReferencePool.Acquire<PoolObject>();
            poolObject.Init(name);
            poolObject.SetPoolRoot(poolRootTrans);
            PoolObjectsMap.Add(name,poolObject);
            return poolObject;
        }

        public  void ReleasePoolObject(string name)
        {
            var poolObject = LoadPoolObject(name);
            if (poolObject == null)
                return;
            PoolObjectsMap.Remove(name);
            ReferencePool.Release(poolObject);
        }

        public void Init(params object[] param)
        {
            PoolObjectsMap = new Dictionary<string, PoolObject>();
            poolRootTrans = new GameObject("[POOL]").transform;
            Object.DontDestroyOnLoad(poolRootTrans.gameObject);
        }

        public void Update()
        {
        }

        public void Clear()
        {
        }

        public void Dispose()
        {
        }
    }
}