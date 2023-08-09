using UnityEditor;
using UnityEngine;
using System.IO;
namespace HOEngine.Editor
{
    public class BuildAssetPrepareStep :IBuildAssetStep
    {
        public string BuildAssetsStepName => "BuildAssetPrepareStep";
        private const string BundleFolderName = "Bundles";
        public ReturnCode Run(IBuildAssetContent content)
        {
            var PlatFormFolder = GetFolderNameByPlatForm(content);
            if (string.IsNullOrEmpty(PlatFormFolder))
                return ReturnCode.SuccessNotReturn;
            var bundlePath = Application.dataPath + "/../../../"+BundleFolderName + "/"+PlatFormFolder;
    
            if (!Directory.Exists(bundlePath))
            {
                Directory.CreateDirectory(bundlePath);
            }
            
            BuildAssetResult.PushBuildAssetResult(BuildAssetsStepName,bundlePath);
            
            return ReturnCode.Success;
        }

        private string GetFolderNameByPlatForm(IBuildAssetContent content)
        {
            switch (content.Target)
            {
                case BuildTarget.StandaloneOSX:
                    return "expOSX";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "expPC";
                case BuildTarget.iOS:
                    return "expIOS";
                case BuildTarget.Android:
                    return "expAndroid";
            }
            return string.Empty;
        }
       
    }
}