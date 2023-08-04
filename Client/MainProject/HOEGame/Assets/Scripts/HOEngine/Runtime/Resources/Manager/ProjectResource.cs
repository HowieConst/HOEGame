

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    public class ProjectResource :IProjectResource
    {
        public bool HasAsset(string assetName)
        {
            return false;
        }

        public void LoadAssetAsync(string name,int priority, EAssetType assetType, Action<string, Object> callBack)
        {
            if (assetType == EAssetType.Prefab)
            {
                ResourceManager.Instacne().LoadInstance(name,priority,assetType,callBack);
            }
            else
            {
                ResourceManager.Instacne().LoadAsset(name,priority,assetType,callBack);
            }
        }
        
    }
}


