using UnityEditor;
using UnityEngine;
using System.IO;
namespace HOEngine.Editor
{
    public class BuildAssetPrepareStep :IBuildAssetStep
    {
        public EBuildAssetStep BuildStep => EBuildAssetStep.BuildAssetPrepareStep;
        
        private const string BundleFolderName = "Bundles";

        public ReturnCode Run(IBuildAssetContent content)
        {
            var platFormFolder = GetFolderNameByPlatForm(content);
            if (string.IsNullOrEmpty(platFormFolder))
                return ReturnCode.SuccessNotReturn;
            var bundlePath = Application.dataPath + "/../../../"+BundleFolderName + "/"+platFormFolder;
    
            if (!Directory.Exists(bundlePath))
            {
                Directory.CreateDirectory(bundlePath);
            }
            
            BuildAssetResult.PushBuildAssetResult(BuildStep,bundlePath);
            
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