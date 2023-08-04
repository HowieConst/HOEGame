using System.Collections.Generic;
using UnityEngine;

namespace HOEngine.Resources
{
    internal class PoolObject : ResourceObject
    {

        private const int MAX_CACHE_COUNT = 10;
        
        private Queue<GameObject> FreeGoQueue;
        
        private Transform PoolRootTrans;

        private GameObject ReleaseGo;


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

        public void SetPoolRoot(Transform rootTrans)
        {
            PoolRootTrans = rootTrans;
        }
        
        public GameObject GetInstance()
        {
            if (FreeGoQueue.Count > 0)
            {
                return FreeGoQueue.Dequeue();
            }
            return Object.Instantiate(PrefabSource);
        }

        private void UnLoadInstance(GameObject gameObject)
        {
            if (gameObject != null)
            {
                if (FreeGoQueue.Count >= MAX_CACHE_COUNT)
                {
                    //销毁了
                    Object.Destroy(gameObject);
                }
                else
                {
                    FreeGoQueue.Enqueue(gameObject);
                    gameObject.transform.SetParent(PoolRootTrans,false);
                    gameObject.SetActive(false);
                }
            }
            SubReference();
       
            //引用计数小于0 了 直接卸载资源了
        }

        public override void UnLoad(GameObject go = null)
        {
            if(go == null)
                return;
            SubReference();
            if (ReferenceCount <= 0)
            {
                PoolManager.Instacne().ReleasePoolObject(Name);
                ReleaseGo = go;
                Release();
            }
        }

        public override void Release()
        {
            if(ReleaseGo == null)
                return;
            UnLoadInstance(ReleaseGo);
            ReleaseGo = null;
        }
    }
}