using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;

namespace HOEngine.Editor.PackResources
{
    
    [CreateAssetMenu(menuName ="PackResources/Create Pack Mainfest",fileName ="PackMainfest")]
    public class HOEPackMainfest :SerializedScriptableObject 
    {
        //源目录
        public string SrcDir;
        
        public List<HOEPackItem> PackItems;

    }
}