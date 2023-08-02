using System.Collections.Generic;
using UnityEngine.UI;

namespace HOEngine.Resources
{
    /// <summary>
    /// Asset 对象管理类
    /// </summary>
    internal sealed class AssetManager :Singlton<AssetManager>,IEngineManager
    {
        
        private AssetManager()
        {
            
        }
        private  Dictionary<string, AssetObject> AssetObjectMap;

     
        /// <summary>
        /// 根据名字获取资源对象
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public  AssetObject LoadAsset(string assetName)
        {
            return AssetObjectMap.TryGetValue(assetName, out var assetObject) ? assetObject : null;
        }

        /// <summary>
        /// 创建资源对象
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public  AssetObject CreateAsset(string assetName)
        {
            var assetObject = ReferencePool.Acquire<AssetObject>();
            assetObject.Init(assetName);
            AssetObjectMap.Add(assetName,assetObject);
            return assetObject;
        }

        /// <summary>
        /// 释放资源对象
        /// </summary>
        /// <param name="assetName"></param>
        public  void ReleaseAssets(string assetName)
        {
            var assetObject = LoadAsset(assetName);
            if (assetObject == null)
                return;
            AssetObjectMap.Remove(assetName);
            ReferencePool.Release(assetObject);
        }

        public void Init(params object[] param)
        {
            AssetObjectMap = new Dictionary<string, AssetObject>();
        }

        public void Update()
        {
        }

        public  void Clear()
        {
            foreach (var item in AssetObjectMap)
            {
                ReferencePool.Release(item.Value);
            }
            AssetObjectMap.Clear();
        }

        public void Dispose()
        {
        }
    }
}