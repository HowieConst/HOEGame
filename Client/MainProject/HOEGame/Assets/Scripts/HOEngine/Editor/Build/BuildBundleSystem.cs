using System.Collections.Generic;

namespace HOEngine.Editor
{
    public static class BuildBundleSystem
    {
        private static List<IBuildAssetStep> BuildAssetSteps = new List<IBuildAssetStep>();
        public static void Build(IBuildAssetContent buildAssetContent)
        {
            BuildAssetResult.Start();
            BuildAssetSteps.Clear();
            BuildAssetSteps.Add(new BuildAssetPrepareStep());
            BuildAssetSteps.Add(new CollectAssetToBuildStep());
            BuildAssetSteps.Add(new BuildAssetStep());

            for (int i = 0; i < BuildAssetSteps.Count; i++)
            {
                var buildStep = BuildAssetSteps[i];
                if (buildStep.Run(buildAssetContent) != ReturnCode.Success)
                {
                    //失败输出log
                    return;
                }
            }
            BuildAssetResult.Clear();
        }
    }
}