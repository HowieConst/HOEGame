using HOEngine.Resources;
using UnityEngine;

namespace HOEngine.Resources
{
    internal class BundleObject :ResourceObject
    {
        public AssetBundle BundleData => ObjectSource as AssetBundle;
     
        public override void UnLoad()
        {
            if (BundleData != null)
            {
                BundleData.Unload(false);
            }
        }
    }
}