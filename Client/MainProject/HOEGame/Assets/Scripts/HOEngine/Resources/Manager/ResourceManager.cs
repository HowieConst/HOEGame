using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// 是否准备完成
        /// </summary>
        public static bool IsReady;

        public static EResourceMode ResourceMode;

        private static LinkedList<IAssetLoader> AssetLoaders;



        private static int MAX_LOAD_ASSET_COUNT = 10;

        private static int LoadAssetCount;

        public static void Init(EResourceMode mode)
        {
            ResourceMode = mode;
            AssetLoaders = new LinkedList<IAssetLoader>();
            LoadAssetCount = 0;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName"></param>资源名称
        /// <param name="priority"></param>加载优先级
        /// <param name="assetType"></param>资源类型
        /// <param name="type"></param>类型
        /// <param name="callBack"></param>回调
        public static void LoadAsset(string assetName, int priority, EAssetType assetType, Type type,
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
            AssetObject assetObject = AssetManager.LoadAsset(assetName);
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

            assetObject = AssetManager.CreateAsset(assetName);
            assetObject.AddReference();
            assetObject.AddLoadCallBack(loadCallBack);
            var assetLoader = ReferencePool.Acquire<AssetLoader>();
            assetLoader.Init(assetName,assetType,priority);
            AssetLoaders.AddLast(assetLoader);
            SortByPriority();
        }

        public static void Update()
        {
            
        }

        private static void UpdateLoader()
        {
            if(AssetLoaders.Count <= 0)
                return;
            LoadAssetCount = 0;
            var current = AssetLoaders.First;
            while (current != null)
            {
                LoadAssetCount += 1;
                IAssetLoader assetLoader = current.Value;
                assetLoader.LoadAssetAsync();
                //正在加载的loader
                var loaderStatus = assetLoader.GetLoaderStatus();
                //加载完成
                if (loaderStatus == ELoaderStatus.LoadBundleFinish)
                {
                    LoadAssetCount -= 1;
                    current = current.Next;
                    continue;
                }
                if (loaderStatus == ELoaderStatus.LoadFinish)
                {
                    AssetLoaders.Remove(assetLoader);
                    ReferencePool.Release(assetLoader);
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
        private static void SortByPriority()
        {
            AssetLoaders.Sort((a,b) => a.GetPriority().CompareTo(b.GetPriority()));
            //不包含的暂停
            var current = AssetLoaders.First;
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
                    type = typeof(UnityEngine.Object);
                    break;
            }
            return type;
        }
    }
}