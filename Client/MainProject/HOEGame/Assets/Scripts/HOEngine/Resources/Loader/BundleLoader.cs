using System;
using System.Collections.Generic;
using UnityEngine;

namespace HOEngine.Resources
{
    public class BundleLoader:IResourceLoader
    {
        private string AssetName;
        private EAssetType AssetType;
        private int LoadPriority;
        private ELoaderStatus LoaderStatus;

        private string BundleName;
        private List<string> BundleLoadList;

        private AssetBundleCreateRequest MainBundleRequest;
        private AssetBundleRequest BundleAssetRequest;
        private Dictionary<string,AssetBundleCreateRequest> BundleLoadRequest;


        public void Init(string assetName,EAssetType assetType,int prority)
        {
            AssetName = assetName;
            AssetType = assetType;
            LoadPriority = prority;
            LoaderStatus = ELoaderStatus.Wait;
            BundleName = BundleManager.Instacne().GetBundleName(assetName);
            BundleLoadList = BundleManager.Instacne().GetBundleDependencies(BundleName);
            BundleLoadRequest = new Dictionary<string, AssetBundleCreateRequest>();
        }
        public void Clear()
        {
            
        }

        public void LoadAssetAsync()
        {
            switch (LoaderStatus)
            {
                case ELoaderStatus.None:
                    break;
                case ELoaderStatus.Wait:
                    OnLoadBundleDependenciesAsync();
                    break;
                case ELoaderStatus.LoadAsset:
                    OnLoadAssetASync();
                    break;
                case ELoaderStatus.LoadFinish:
                    break;
                case ELoaderStatus.LoadBundleDependencyFinish:
                    OnLoadMainBundle();
                    break;
                case ELoaderStatus.LoadBundleFinish:
                    LoaderStatus = ELoaderStatus.LoadAsset;
                    break;
                case ELoaderStatus.UnLoad:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnLoadBundleDependenciesAsync()
        {
            if (BundleLoadList == null || BundleLoadList.Count <= 0)
            {
                LoaderStatus = ELoaderStatus.LoadBundleDependencyFinish;
                return;
            }
            foreach (var item in BundleLoadList)
            {
                var bundleObject = BundleManager.Instacne().LoadBundleObject(item);
                if (bundleObject == null)
                {
                    bundleObject = BundleManager.Instacne().CreateBundleObject(item);
                    bundleObject.AddReference();
                }

                if (!BundleLoadRequest.ContainsKey(item))
                {
                    var bundleRequest = AssetBundle.LoadFromFileAsync(item);
                    BundleLoadRequest.Add(item,bundleRequest);
                }
            }

            foreach (var item in BundleLoadRequest)
            {
                if (!item.Value.isDone)
                {
                    return;
                }

                var bundleObject = BundleManager.Instacne().LoadBundleObject(item.Key);
                if (bundleObject != null)
                {
                    bundleObject.SetResourceObject(item.Value.assetBundle);
                }
            }

            LoaderStatus = ELoaderStatus.LoadBundleDependencyFinish;
            foreach (var item in BundleLoadList)
            {
                var bundleObject = BundleManager.Instacne().LoadBundleObject(item);
                if (bundleObject == null)
                {
                    bundleObject = BundleManager.Instacne().CreateBundleObject(item);
                    bundleObject.AddReference();
                }

                if (!BundleLoadRequest.ContainsKey(item))
                {
                    var bundleRequest = AssetBundle.LoadFromFileAsync(item);
                    BundleLoadRequest.Add(item,bundleRequest);
                }
            }
        }

        private void OnLoadMainBundle()
        {
            var bundleObject = BundleManager.Instacne().LoadBundleObject(BundleName);
            if (bundleObject == null)
            {
                bundleObject = BundleManager.Instacne().CreateBundleObject(BundleName);
                bundleObject.AddReference();
            }

            MainBundleRequest ??= AssetBundle.LoadFromFileAsync(BundleName);
            if (MainBundleRequest.isDone)
            {
                bundleObject.SetResourceObject(MainBundleRequest.assetBundle);
                LoaderStatus = ELoaderStatus.LoadFinish;
            }
        }

        private void OnLoadAssetASync()
        {
            var bundleObject = BundleManager.Instacne().LoadBundleObject(BundleName);
            if (bundleObject != null)
            {
                if (bundleObject.BundleData != null)
                {
                    BundleAssetRequest ??= bundleObject.BundleData.LoadAssetAsync(AssetName);
                }
            }

            if (BundleAssetRequest != null && BundleAssetRequest.isDone)
            {
                var assetObject = AssetManager.Instacne().LoadAsset(AssetName);
                if (assetObject != null)
                {
                    assetObject.SetResourceObject(BundleAssetRequest.asset);
                }
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
  
    }
}