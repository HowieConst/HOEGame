using System.Collections.Generic;
using UnityEngine;
namespace HOEngine.Resources
{
    /// <summary>
    /// 资源对象
    /// </summary>
    public class AssetObject :IReference
    {
        /// <summary>
        /// 资源对象
        /// </summary>
        public Object Asset { get; protected set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string AssetName { get; private set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public EAssetType AssetType{ get; private set; }

        /// <summary>
        /// 引用次数
        /// </summary>
        public int ReferenceCount { get; private set; }

        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool IsLoaded { get; private set; }

        protected Queue<AssetLoadCallBack> CallBackQueue;


        /// <summary>
        /// 初始化 
        /// </summary>
        /// <param name="assetName"></param>
        public void Init(string assetName)
        {
            AssetName = assetName;
        }

        /// <summary>
        /// 添加引用计数
        /// </summary>
        public void AddReference()
        {
            ReferenceCount++;
        }

        /// <summary>
        /// 减少引用计数
        /// </summary>
        public void SubReference()
        {
            ReferenceCount--;
        }

        /// <summary>
        /// 加载完成设置资源对象
        /// </summary>
        /// <param name="assetType"></param>
        /// <param name="assetObject"></param>
        public void SetAssetObject(EAssetType assetType,Object assetObject)
        {
            IsLoaded = true;
            Asset = assetObject;
            AssetType = assetType;

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
                    callBack.Invoke(Asset);
                }
            }
        }
        
        /// <summary>
        /// 销毁资源
        /// </summary>
        public void DestroyAsset()
        {
            if(Asset == null)
                return;
            UnityEngine.Resources.UnloadAsset(Asset);
        }
        
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            IsLoaded = false;
            ReferenceCount = 0;
            AssetName = "";
            Asset = null;
            AssetType = EAssetType.Object;
            CallBackQueue?.Clear();
            CallBackQueue = null;
        }
    }
}