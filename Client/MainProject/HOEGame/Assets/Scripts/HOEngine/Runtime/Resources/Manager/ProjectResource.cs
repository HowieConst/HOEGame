

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    public class ProjectResource : IProjectResource
    {
        /// <summary>
        /// 资源地址
        /// </summary>
        private static string DEV_RES_PATH_WIN = "/../Bin/Res/";

        //内部的AssetBundle资源
        private static HashSet<string> InsideBundle = null;


        public ProjectResource()
        {
            //初始化读取配置
        }
        

        public bool HasAsset(string assetName)
        {
            switch (ResourceManager.Instacne().ResourceMode)
            {
                case EResourceMode.None:
                    return false;
                case EResourceMode.Editor:
                    break;
                case EResourceMode.AssetBundle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
            return false;
        }

        public ResHandler LoadAssetAsync(string name, EAssetType assetType, ELoadPriority priority,
            Action<string, Object> callBack)
        {
            var resHandler = GetResHandler(name);
            resHandler.LoadAsset(name, assetType, priority, callBack);
            return resHandler;
        }

        private ResHandler GetResHandler(string name)
        {
            ResHandler handler = ReferencePool.Acquire<ResHandler>();
            handler.Init(ReleaseHandler);
            return handler;
        }

        private void ReleaseHandler(ResHandler resHandler)
        {
            ReferencePool.Release(resHandler);
        }

        public static string GetLoadPath(string assetName)
        {
            switch (ResourceManager.Instacne().ResourceMode)
            {
                case EResourceMode.None:
                    return string.Empty;
                case EResourceMode.Editor:
                    return assetName;
                case EResourceMode.AssetBundle:
                    return GetBundlePath(assetName);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetBundlePath(string assetName)
        {
            string filePath = "";
            var bundleName = BundleManager.Instacne().GetBundleName(assetName);
            filePath = Application.streamingAssetsPath + bundleName;
#if UNITY_EDITOR || UNITY_STANDALONE
            filePath = Application.dataPath + DEV_RES_PATH_WIN + bundleName;
#elif UNITY_ANDROID
            if (InsideBundle == null || !InsideBundle.Contains(bundleName))
            {
                filePath = $"{Application.dataPath}!assets/{bundleName}";
            }
#else
           if (InsideBundle == null || !InsideBundle.Contains(bundleName))
            {
                filePath = filePath = $"{Application.dataPath}/Raw/{bundleName}";
            }
#endif
            return filePath;
        }
    }
}


