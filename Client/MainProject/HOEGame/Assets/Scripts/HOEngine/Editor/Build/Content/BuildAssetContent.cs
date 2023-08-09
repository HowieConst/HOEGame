using System.Collections.Generic;
using HOEngine.Editor.PackResources;
using UnityEditor;

namespace HOEngine.Editor
{
    /// <summary>
    /// BuildAsset 上下文参数
    /// </summary>
    public class BuildAssetContent :IBuildAssetContent
    {

        public BuildTarget Target { get; private set; }
        public List<HOEPackMainfest> PackMainfests { get; private set; }
        public List<string> PackMainfestPath { get; }
        public BuildAssetBundleOptions BundleOptions { get; private set; }
        

        public BuildAssetContent(BuildTarget target,BuildAssetBundleOptions bundleOptions,List<string> packMainfestPath )
        {
            Target = target;
            BundleOptions = bundleOptions;
            PackMainfestPath = packMainfestPath;

        }
        
    }
}