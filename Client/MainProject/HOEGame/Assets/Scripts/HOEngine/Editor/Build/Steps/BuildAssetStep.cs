using System.Collections.Generic;
using UnityEditor;

namespace HOEngine.Editor
{
    public class BuildAssetStep :IBuildAssetStep
    {
        public string BuildAssetsStepName => "BuildAssetStep";
        public ReturnCode Run(IBuildAssetContent content)
        {
            var collectAssetResult = BuildAssetResult.PopBuildAssetResult("CollectAssetToBuildStep");
            if (collectAssetResult == null || collectAssetResult.Length == 0)
            {
                return ReturnCode.SuccessNotReturn;
            }

            var assetBundleBuilder = collectAssetResult[0] as List<AssetBundleBuild>;
            if (assetBundleBuilder == null || assetBundleBuilder.Count <= 0)
                return ReturnCode.SuccessNotReturn;
            
            var buildOutPath = BuildAssetResult.PopBuildAssetResult("BuildAssetPrepareStep");
            if (buildOutPath == null || buildOutPath.Length == 0)
                return ReturnCode.SuccessNotReturn;
            var outPath = buildOutPath[0] as string;
            if (string.IsNullOrEmpty(outPath))
                return ReturnCode.SuccessNotReturn;

            var bundleBuildOptions = content.BundleOptions;
            var buildTarget = content.Target;

            BuildPipeline.BuildAssetBundles(outPath, assetBundleBuilder.ToArray(), bundleBuildOptions, buildTarget);
            
            return ReturnCode.Success;
        }
    }
}