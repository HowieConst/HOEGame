using System.Collections.Generic;

namespace HOEngine.Resources
{
    internal sealed class BundleManager : Singlton<BundleManager> , IEngineManager
    {
        
        private BundleManager()
        {
            
        }
        
        private Dictionary<string, BundleObject> BundleMaps;

        private Dictionary<string, string> AssetBundleReferenceMap;

        private Dictionary<string, List<string>> BundleDependencyMap;



        public void Init(params object[] param)
        {
            BundleMaps = new Dictionary<string, BundleObject>();
            AssetBundleReferenceMap = new Dictionary<string, string>();
            BundleDependencyMap = new Dictionary<string, List<string>>();
        }

        public void Update()
        {
        }
        internal BundleObject CreateBundleObject(string name)
        {
            BundleObject bundleObject = ReferencePool.Acquire<BundleObject>();
            bundleObject.Init(name);
            BundleMaps.Add(name,bundleObject);
            return bundleObject;
        }

        internal BundleObject LoadBundleObject(string name)
        {
            return BundleMaps.TryGetValue(name, out var bundleObject) ? bundleObject : null;
        }
        public string GetBundleName(string assetName)
        {
            return AssetBundleReferenceMap.TryGetValue(assetName, out var bundleName) ? bundleName : string.Empty;
        }

        public List<string> GetBundleDependencies(string bundleName)
        {
            if (BundleDependencyMap.TryGetValue(bundleName, out var dependencies))
            {
                return dependencies;
            }

            return null;
        }

        public  void ReleaseBundle(string name)
        {
            var bundleObject = LoadBundleObject(name);
            if (bundleObject == null)
                return;
            BundleMaps.Remove(name);
            ReferencePool.Release(bundleObject);
        }

        public void Clear()
        {
            foreach (var item in BundleMaps)
            {
                ReferencePool.Release(item.Value);
            }
            BundleMaps.Clear();
        }
        public void Dispose()
        {
        }
    }
}