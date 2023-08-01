using HOEngine.Resources;
using UnityEngine;

namespace HOEngine
{
    public class BundleObject :IReference, IResourceObject
    {
        public string Name { get; private set; }
        
        public Object ResourceObject { get; private set; }
        
        public void Init(string name)
        {
            Name = name;
        }

        public int ReferenceCount { get; private set; }
        
        public bool IsLoaded { get; private set; }
        
        public AssetBundle BundleData => ResourceObject as AssetBundle;

        public void SetBunldeObject(AssetBundle assetBundle)
        {
            ResourceObject = assetBundle;
        }
        
        public void AddReference()
        {
            ReferenceCount++;
        }

        public void SubReference()
        {
            ReferenceCount--;
        }

        public void Clear()
        {
            Name = "";
            ResourceObject = null;
            ReferenceCount = 0;
            IsLoaded = false;
        }
    }
}