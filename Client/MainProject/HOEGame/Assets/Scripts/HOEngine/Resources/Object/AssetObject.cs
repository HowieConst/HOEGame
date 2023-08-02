using System.Collections.Generic;
using UnityEngine;
namespace HOEngine.Resources
{
    /// <summary>
    /// 资源对象
    /// </summary>
    internal class AssetObject :ResourceObject
    {
        /// <summary>
        /// 销毁资源
        /// </summary>
        public override void UnLoad()
        {
            if(ObjectSource == null)
                return;
            UnityEngine.Resources.UnloadAsset(ObjectSource);
        }
   
    }
}