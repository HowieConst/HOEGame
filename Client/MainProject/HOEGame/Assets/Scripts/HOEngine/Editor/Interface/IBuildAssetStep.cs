using System.Collections.Generic;
using HOEngine.Editor.PackResources;

namespace HOEngine.Editor
{
    public interface IBuildAssetStep 
    {
        string BuildAssetsStepName { get; }
        
        ReturnCode Run(IBuildAssetContent content);
    }
}