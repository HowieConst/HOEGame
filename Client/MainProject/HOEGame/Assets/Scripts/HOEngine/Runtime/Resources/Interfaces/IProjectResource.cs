
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    /// <summary>
    /// 资源接口
    /// </summary>
    public interface IProjectResource
    {
        /// <summary>
        /// 资源是否存在
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        bool HasAsset(string assetName);

        void LoadAssetAsync(string name,int priority, EAssetType assetType, Action<string, Object> action);
    }
}

