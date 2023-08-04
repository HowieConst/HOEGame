using System.Collections.Generic;
using System.Linq;
using HOEngine.Resources;
using UnityEngine;

namespace HOEngine.Resources
{
    internal class BundleObject :ResourceObject
    {
        public AssetBundle BundleData => ObjectSource as AssetBundle;
     
        public override void UnLoad(GameObject go = null)
        {
            if(BundleData == null)
                return;
            SubReference();
            string bundleName = BundleManager.Instacne().GetBundleName(Name);
            List<string> dependencies = BundleManager.Instacne().GetBundleDependencies(bundleName);
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    var bundleObject = BundleManager.Instacne().LoadBundleObject(dependency);
                    bundleObject?.UnLoad();
                }
            }
            if (ReferenceCount <= 0)
            {
                BundleManager.Instacne().ReleaseBundle(bundleName);
                Release();
            }
        }

        public override void Release()
        {
            BundleData.Unload(false);
        }
    }
}