using System;
using HOEngine.Log;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    /// <summary>
    /// 资源加载句柄
    /// </summary>
    public class ResHandler : IResHandler,IReference
    {
        private string AssetName;
        private EAssetType AssetType;
        private Object ResourceObject;
        private EReleaseType ReleaseType;
        private bool IsLoaded;
        private bool IsRelease;
        private Action<string, Object> LoadCallBack;
        private Action<ResHandler> ReleaseCallBack;

        public void Init(Action<ResHandler> releaseCallBack)
        {
            ReleaseCallBack = releaseCallBack;
        }

        internal void LoadAsset(string assetName, EAssetType assetType,ELoadPriority priority, Action<string, UnityEngine.Object> callback)
        {
            AssetName = assetName;
            AssetType = assetType;
            ResourceObject = null;
            IsLoaded = false;
            IsRelease = false;
            LoadCallBack = callback;


            if (AssetType == EAssetType.Prefab)
            {
                ReleaseType = EReleaseType.UnloadInstance;
                ResourceManager.Instacne().LoadInstance(assetName,assetType,priority,LoadAssetCallback);
            }
            else
            {
                ReleaseType = EReleaseType.UnloadAsset;
                ResourceManager.Instacne().LoadAsset(assetName,assetType,priority,LoadAssetCallback);
            }
        }
        private void LoadAssetCallback(string name, UnityEngine.Object asset)
        {
            if (AssetName != name)
            {
                LogManager.Instacne().LogError($"无效的资源名{name}", ELogChannel.Resource);
                return;
            }
            if (IsRelease)
            { 
                ReleaseAsset(asset);
                return;
            }

            ResourceObject = asset;
            IsLoaded = true;

            if (LoadCallBack != null)
            {
                LoadCallBack.Invoke(name,asset);
            }
        }
        private void ReleaseAsset(Object target)
        {
            switch (ReleaseType)
            {
                case EReleaseType.UnloadInstance:
                    ResourceManager.Instacne().UnLoadInstance(AssetName, target as GameObject);
                    break;
                case EReleaseType.UnloadAsset:
                    ResourceManager.Instacne().UnLoadAsset(AssetName);
                    break;
            }
            ReleaseCallBack?.Invoke(this);
        }
        
        public void Release()
        {
            if(IsRelease)
                return;
            IsRelease = true;
            if (IsLoaded)
            {
                ReleaseAsset(ResourceObject);
            }
        }

        public void Clear()
        {
           AssetName = "";
           ResourceObject = null;
           IsLoaded = false;
           IsRelease = false;
           LoadCallBack = null;
        }
    }
}