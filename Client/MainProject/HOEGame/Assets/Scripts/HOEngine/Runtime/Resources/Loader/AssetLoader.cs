using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    //资源加载器(Editor下)
    public class AssetLoader :IResourceLoader
    {

        private string AssetName;
        private EAssetType AssetType;
        private ELoadPriority LoadPriority;
        private Object AssetObject;
        private ELoaderStatus LoaderStatus;

        public void Init(string assetName,EAssetType assetType,ELoadPriority prority)
        {
            AssetName = assetName;
            AssetType = assetType;
            LoadPriority = prority;
            LoaderStatus = ELoaderStatus.Wait;
        }
        
        public void LoadAssetAsync()
        {
            switch (LoaderStatus)
            {
                case ELoaderStatus.None:
                    break;
                case ELoaderStatus.Wait:
                    LoaderStatus = ELoaderStatus.LoadAsset;
                    break;
                case ELoaderStatus.LoadAsset:
                    OnLoadAssetAsync();
                    break;
                case ELoaderStatus.LoadFinish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ELoaderStatus GetLoaderStatus()
        {
            return LoaderStatus;
        }

        public ELoadPriority GetPriority()
        {
            return LoadPriority;
        }

        public void Pause()
        {
            
        }

        public bool IsLoaded => LoaderStatus == ELoaderStatus.LoadFinish;

        private void OnLoadAssetAsync()
        {
            #if UNITY_EDITOR
            var type = ResourceManager.GetTypeByAssetType(AssetType);
            AssetObject = AssetDatabase.LoadAssetAtPath(AssetName,type);
            var assetObject = AssetManager.Instacne().LoadAsset(AssetName);
            assetObject?.SetResourceObject(AssetObject);
            #endif
            LoaderStatus = ELoaderStatus.LoadFinish;
        }

        public void Clear()
        {
          AssetName = "";
          LoadPriority = 0;
          AssetObject = null;
          LoaderStatus = ELoaderStatus.None;
        }
    }
}