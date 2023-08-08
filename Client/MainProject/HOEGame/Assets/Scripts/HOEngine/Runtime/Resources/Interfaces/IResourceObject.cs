using System;
using Object = UnityEngine.Object;
namespace HOEngine.Resources
{
    internal interface IResourceObject
    {
        string Name { get;}

        Object ResourceObject { get;}

        void SetResourceObject(EAssetType assetType, Object assetObject);

        void Init(string name);
        
        int ReferenceCount { get;}
        
        bool IsLoaded { get;}

        void AddReference();

        void SubReference();

    }
}