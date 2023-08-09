using System.Collections.Generic;
using System.IO;
using HOEngine.Editor.PackResources;
using UnityEditor;

namespace HOEngine.Editor
{
    public class CollectAssetToBuildStep :IBuildAssetStep
    {
        public string BuildAssetsStepName => "CollectAssetToBuildStep";
        private const string SharedBundleName = "Shared_{0}";
        private const int SharedCount = 10;
        public ReturnCode Run(IBuildAssetContent content)
        {
            
            if (content.PackMainfestPath == null || content.PackMainfestPath.Count == 0)
                return ReturnCode.SuccessNotReturn;


            var result = new Dictionary<string, List<string>>();
            var dependencies = new Dictionary<string, HashSet<string>>();

            var packMainfestFiles = new List<HOEPackMainfest>();

            for (int i = 0; i < content.PackMainfestPath.Count; i++)
            {
                var path = content.PackMainfestPath[i];
                var packMainfest = AssetDatabase.LoadAssetAtPath<HOEPackMainfest>(path);
                if (packMainfest != null)
                {
                    packMainfestFiles.Add(packMainfest);
                }
            }

            if (packMainfestFiles.Count == 0)
            {
                return ReturnCode.SuccessNotReturn;
            }
            
            foreach (var packItem in packMainfestFiles)
            {
               packItem.PackAllAssets(ref result,ref dependencies);
            }

            var sharedAsset = new List<string>();
            foreach (var item in dependencies)
            {
                //多个
                if (item.Value.Count >= 2)
                {
                    var assetpath = Path.GetFullPath(item.Key);
                    var fileInfo = new FileInfo(assetpath);
                    if (fileInfo.Length > 2 * 1024 * 1024)
                    {
                        sharedAsset.Add(item.Key);
                    }
                }
            }

            var sharedBundle = new Dictionary<string, List<string>>();
            if (sharedAsset.Count > 0)
            {
                int sharedIndex = 1;
                for (int i = 0; i < sharedAsset.Count; i++)
                {
                    var sharedBundleName = string.Format(SharedBundleName, sharedIndex);
                    if (!sharedBundle.ContainsKey(sharedBundleName))
                    {
                        sharedBundle.Add(sharedBundleName,new List<string>());
                    }
                    sharedBundle[sharedBundleName].Add(sharedAsset[i]);

                    if (i >= sharedIndex * SharedCount-1)
                    {
                        sharedIndex++;
                    }
                }
            }
            
            var assetBunldeBuilderList = new List<AssetBundleBuild>();
            
            foreach (var item in sharedBundle)
            {
                var bundleBuilder = new AssetBundleBuild
                {
                    assetBundleName = item.Key,
                    assetNames = item.Value.ToArray()
                };
                assetBunldeBuilderList.Add(bundleBuilder);
            }
            foreach (var item in result)
            {
                var bundleBuilder = new AssetBundleBuild
                {
                    assetBundleName = item.Key,
                    assetNames = item.Value.ToArray()
                };
                assetBunldeBuilderList.Add(bundleBuilder);
            }
            
            BuildAssetResult.PushBuildAssetResult(BuildAssetsStepName,assetBunldeBuilderList);
            
            return ReturnCode.Success;
        }
    }
}