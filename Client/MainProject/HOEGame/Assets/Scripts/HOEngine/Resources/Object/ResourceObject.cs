using System.Collections.Generic;
using UnityEngine;

namespace HOEngine.Resources
{
    internal abstract class ResourceObject :IReference
    {
        public string Name { get;protected set; }

        protected Object ObjectSource;
        
        protected int ReferenceCount;
        
        public bool IsLoaded {get;protected set;}
        
        private Queue<AssetLoadCallBack> CallBackQueue;

        public abstract void UnLoad();

        public virtual Object GetObject()
        {
            return ObjectSource;
        }
        

        public virtual void Init(string name)
        {
            Name = name;
        }

        public virtual void SetResourceObject( Object resourceObject)
        {
            ObjectSource = resourceObject;
            IsLoaded = true;
        }

        public virtual void AddReference()
        {
            ReferenceCount++;
        }

        public virtual void SubReference()
        {
            ReferenceCount--;
        }

        /// <summary>
        /// 添加加载资源的回调
        /// </summary>
        /// <param name="assetLoadCallBack"></param>
        public void AddLoadCallBack(AssetLoadCallBack assetLoadCallBack)
        {
            CallBackQueue ??= new Queue<AssetLoadCallBack>();
            CallBackQueue.Enqueue(assetLoadCallBack);
        }

        /// <summary>
        /// 加载完成执行完成回调
        /// </summary>
        public void ExecuteLoadCallBack()
        {
            if (CallBackQueue != null && CallBackQueue.Count > 0)
            {
                while (CallBackQueue.Count > 0)
                {
                    var callBack = CallBackQueue.Dequeue();
                    callBack.Invoke(ObjectSource);
                }
            }
        }
        
        public virtual void Clear()
        {
            Name = "";
            ObjectSource = null;
            ReferenceCount = 0;
            IsLoaded = false;
            CallBackQueue?.Clear();
            CallBackQueue = null;
        }
    }
}