using System.Threading;

namespace HOEngine.Editor
{
    public class BuildAssetBundleTask:IBuildTask
    {
        public string BuildStepName => this.GetType().Name;
        public IBuildParameters GetBuildParameters()
        {
            return null;
        }
    }
}