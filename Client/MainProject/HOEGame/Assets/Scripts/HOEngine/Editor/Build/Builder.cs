using System.Collections.Generic;
using UnityEditor;

namespace HOEngine.Editor
{
    public static class Builder
    {
        [MenuItem("Tools/BuildBundleTest")]
        public static void BuildAssetBundle()
        {
            //test

            var buildContet = new BuildAssetContent(BuildTarget.StandaloneWindows64,
                BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
                new List<string>(){@"Assets\Scripts\HOEngine\Editor\PackResources\PakMainfests\PackMainfest.asset"});
            
            BuildBundleSystem.Build(buildContet);
        }
    }
}