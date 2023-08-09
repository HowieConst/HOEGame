namespace HOEngine.Editor
{
    public class GenerateBuildMapStep : IBuildAssetStep
    {
        public EBuildAssetStep BuildStep => EBuildAssetStep.GenerateBuildMapStep;

        public ReturnCode Run(IBuildAssetContent content)
        {
            throw new System.NotImplementedException();
        }
    }
}