using System.Collections.Generic;
using HOEngine.Editor.PackResources;
using UnityEditor;

namespace HOEngine.Editor
{
    public interface IBuildAssetContent:IBuildContent
    {

        List<string> PackMainfestPath { get; }

        BuildAssetBundleOptions BundleOptions { get; }
        
    }
}