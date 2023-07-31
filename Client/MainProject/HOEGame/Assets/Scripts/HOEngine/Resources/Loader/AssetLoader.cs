using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    //资源加载器 包括bundle加载
    public class AssetLoader :IAssetLoader
    {

        private string AssetName;
        private EAssetType AssetType;
        private int LoadPriority;
        private Object AssetObject;
        public ELoaderStatus LoaderStatus;

        public void Init(string assetName,EAssetType assetType,int prority)
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
                    break;
                case ELoaderStatus.LoadBundle:
                    OnLoadBundleAsync();
                    break;
                case ELoaderStatus.LoadAsset:
                    OnLoadAssetAsync();
                    break;
                case ELoaderStatus.LoadFinish:
                    break;
                case ELoaderStatus.UnLoad:
                    break;
                case ELoaderStatus.LoadBundleFinish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ELoaderStatus GetLoaderStatus()
        {
            return LoaderStatus;
        }

        public int GetPriority()
        {
            return LoadPriority;
        }

        public void Pause()
        {
            
        }


        //加载Bundle
        private void OnLoadBundleAsync()
        {
            switch (ResourceManager.ResourceMode)
            {
                case EResourceMode.Editor:
                    LoaderStatus = ELoaderStatus.LoadAsset;
                    break;
                case EResourceMode.AssetBundle:
                    //LoadBundle
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnLoadAssetAsync()
        {
            switch (ResourceManager.ResourceMode)
            {
                case EResourceMode.Editor:
                    var type = ResourceManager.GetTypeByAssetType(AssetType);
                    AssetObject = AssetDatabase.LoadAssetAtPath(AssetName,type);
                    LoaderStatus = ELoaderStatus.LoadFinish;
                    break;
                case EResourceMode.AssetBundle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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