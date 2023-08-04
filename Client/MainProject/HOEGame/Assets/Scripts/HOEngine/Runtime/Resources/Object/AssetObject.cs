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
        public override void UnLoad(GameObject go = null)
        {
            if(ObjectSource == null)
                return;
            SubReference();
            if (ReferenceCount <= 0)
            {
                AssetManager.Instacne().ReleaseAssets(Name);
                Release();
            }
        }

        public override void Release()
        {
            UnityEngine.Resources.UnloadAsset(ObjectSource);
        }
    }
}