using System.Collections.Generic;
using UnityEngine;

namespace HOEngine.Resources
{
    internal class PoolObject : ResourceObject
    {

        private const int MAX_CACHE_COUNT = 10;
        
        private Queue<GameObject> FreeGoQueue;

        private GameObject PrefabSource => ObjectSource as GameObject;

        public override void Init(string name)
        {
            Name = name;
            FreeGoQueue = new Queue<GameObject>(MAX_CACHE_COUNT);
        }
       
        public override void Clear()
        {
            base.Clear();
        }
        
        public GameObject GetInstance()
        {
            if (FreeGoQueue.Count > 0)
            {
                return FreeGoQueue.Dequeue();
            }
            return Object.Instantiate(PrefabSource);
        }

        public override void UnLoad()
        {
        }
    }
}