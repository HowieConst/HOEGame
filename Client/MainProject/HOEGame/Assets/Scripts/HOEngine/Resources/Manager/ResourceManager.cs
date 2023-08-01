using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public  class ResourceManager :Singlton<ResourceManager>,IEngineManager
    {
        /// <summary>
        /// 是否准备完成
        /// </summary>
        public  bool IsReady;

        public  EResourceMode ResourceMode;

        private LinkedList<IResourceLoader> ResLoaders;

        private const  int MAX_LOAD_ASSET_COUNT = 10;

        private  int LoadAssetCount;


        #region Interface
        public void Init(params object[] param)
        {
            ResLoaders = new LinkedList<IResourceLoader>();
            LoadAssetCount = 0;
            if (param.Length > 0)
            {
                if (param[0] != null)
                {
                    ResourceMode = param[0] as EResourceMode? ?? EResourceMode.None;
                }
            }

            if (ResourceMode == EResourceMode.None)
            {
                Debug.LogError("初始化失败---");
            }
        }

        public void Update()
        {
        }

        public void Clear()
        {
        }

        public void Dispose()
        {
        }
        

        #endregion
        

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName"></param>资源名称
        /// <param name="priority"></param>加载优先级
        /// <param name="assetType"></param>资源类型
        /// <param name="type"></param>类型
        /// <param name="callBack"></param>回调
        public  void LoadAsset(string assetName, int priority, EAssetType assetType,
            Action<string, Object> callBack)
        {
            AssetLoadCallBack loadCallBack = ReferencePool.Acquire<AssetLoadCallBack>();
            loadCallBack.Init(assetName,callBack);
            
            if (string.IsNullOrEmpty(assetName))
            {
                loadCallBack.Invoke(null);
                return;
            }

            //获取资源引用对象
            AssetObject assetObject = AssetManager.Instacne().LoadAsset(assetName);
            if (assetObject != null)
            {
                //存在资源对象 有可能是加载完成或者正在加载
                assetObject.AddReference();
                //加载完成处理
                if (assetObject.IsLoaded)
                {
                    //todo:
                    
                }
                else
                {
                    //todo:没加载完成添加回调
                    assetObject.AddLoadCallBack(loadCallBack);
                }
                return;
            }

            assetObject = AssetManager.Instacne().CreateAsset(assetName);
            assetObject.AddReference();
            assetObject.AddLoadCallBack(loadCallBack);


            if (ResourceMode == EResourceMode.AssetBundle)
            {
                var assetLoader = ReferencePool.Acquire<AssetLoader>();
                assetLoader.Init(assetName,assetType,priority);
                ResLoaders.AddLast(assetLoader);
            }
            else
            {
                var bundleLoader = ReferencePool.Acquire<BundleLoader>();
                bundleLoader.Init(assetName,assetType,priority);
                ResLoaders.AddLast(bundleLoader);
            }
            SortByPriority();
        }

        private void UpdateLoader()
        {
            if(ResLoaders.Count <= 0)
                return;
            LoadAssetCount = 0;
            var current = ResLoaders.First;
            while (current != null)
            {
                LoadAssetCount += 1;
                IResourceLoader resLoader = current.Value;
                resLoader.LoadAssetAsync();
                //正在加载的loader
                var loaderStatus = resLoader.GetLoaderStatus();
                //加载完成
                if (loaderStatus == ELoaderStatus.LoadBunldeFinish)
                {
                    LoadAssetCount -= 1;
                    current = current.Next;
                    continue;
                }
                if (loaderStatus == ELoaderStatus.LoadFinish)
                {
                    ResLoaders.Remove(resLoader);
                    ReferencePool.Release(resLoader);
                    continue;
                }

                if (LoadAssetCount >= MAX_LOAD_ASSET_COUNT)
                {
                    break;
                }

                current = current.Next;
            }
        }

        /// <summary>
        /// 根据优先级进行排序 待测试
        /// </summary>
        private  void SortByPriority()
        {
            ResLoaders.Sort((a,b) => a.GetPriority().CompareTo(b.GetPriority()));
            //不包含的暂停
            var current = ResLoaders.First;
            var count = 0;
            //保证每帧最多加载最大数量
            while (current != null)
            {
                count++;
                if (count > MAX_LOAD_ASSET_COUNT)
                {
                    current.Value.Pause();
                }
                current = current.Next;
            }
        }
        
        internal static Type GetTypeByAssetType(EAssetType assetType)
        {
            Type type = null;
            switch (assetType)
            {
                case EAssetType.Texture:
                    type = typeof(UnityEngine.Texture);
                    break;
                case EAssetType.SpriteAtlas:
                    type = typeof(UnityEngine.U2D.SpriteAtlas);
                    break;
                case EAssetType.TextAsset:
                    type = typeof(UnityEngine.TextAsset);
                    break;
                case EAssetType.Material:
                    type = typeof(UnityEngine.Material);
                    break;
                case EAssetType.ScriptableObject:
                    type = typeof(UnityEngine.ScriptableObject);
                    break;
                case EAssetType.Prefab:
                    type = typeof(UnityEngine.GameObject);
                    break;
                default:
                    type = typeof(Object);
                    break;
            }
            return type;
        }

    }
}