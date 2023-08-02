using System.Collections.Generic;

namespace HOEngine.Resources
{
    public class PoolManager :Singlton<PoolManager>,IEngineManager
    {

        private Dictionary<string, PoolObject> PoolObjectsMap;

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
            poolObject.AddReference();
            PoolObjectsMap.Add(name,poolObject);
            return poolObject;
        }

        internal PoolObject EnSurePoolObject(string name)
        {
            var poolObject = LoadPoolObject(name) ?? CreatePoolObject(name);
            return poolObject;
        }

        public void Init(params object[] param)
        {
            PoolObjectsMap = new Dictionary<string, PoolObject>();
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