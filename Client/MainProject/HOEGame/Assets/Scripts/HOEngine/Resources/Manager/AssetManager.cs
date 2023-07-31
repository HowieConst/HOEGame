using System.Collections.Generic;

namespace HOEngine.Resources
{
    /// <summary>
    /// Asset 对象管理类
    /// </summary>
    internal static class AssetManager
    {
        private static Dictionary<string, AssetObject> AssetObjectMap;

        static AssetManager()
        {
            AssetObjectMap = new Dictionary<string, AssetObject>();
        }

        /// <summary>
        /// 根据名字获取资源对象
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static AssetObject LoadAsset(string assetName)
        {
            return AssetObjectMap.TryGetValue(assetName, out var assetObject) ? assetObject : null;
        }

        /// <summary>
        /// 创建资源对象
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static AssetObject CreateAsset(string assetName)
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
        public static void ReleaseAsset(string assetName)
        {
            var assetObject = LoadAsset(assetName);
            if (assetObject == null)
                return;
            AssetObjectMap.Remove(assetName);
            ReferencePool.Release(assetObject);
        }

        public static void Clear()
        {
            foreach (var item in AssetObjectMap)
            {
                ReferencePool.Release(item.Value);
            }
            AssetObjectMap.Clear();
        }
    }
}