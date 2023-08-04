namespace HOEngine.Resources
{
    internal interface IResourceLoader:IReference
    {
        void LoadAssetAsync();

        ELoaderStatus GetLoaderStatus();

        int GetPriority();

    }
}