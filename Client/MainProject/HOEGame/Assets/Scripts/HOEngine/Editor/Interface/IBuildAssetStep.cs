using System.Collections.Generic;
using HOEngine.Editor.PackResources;

namespace HOEngine.Editor
{
    public interface IBuildAssetStep 
    {
        EBuildAssetStep BuildStep { get; }
        
        ReturnCode Run(IBuildAssetContent content);
    }
}