using System;
using Object = UnityEngine.Object;

namespace HOEngine.Resources
{
    public class AssetLoadCallBack: IReference
    {
        public string AssetName { get; private set; }

        private Action<string, Object> LoadCallBack;

        public void Init(string assetName,Action<string,Object> callBack)
        {
            AssetName = assetName;
            LoadCallBack = callBack;
        }

        public void Invoke(Object assetObject)
        {
            try
            {
                if (LoadCallBack != null)
                {
                    LoadCallBack.Invoke(AssetName, assetObject);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                ReferencePool.Release(this);
            }
        }
        public void Clear()
        {
            AssetName = "";
            LoadCallBack = null;
        }
    }
}