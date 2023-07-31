namespace HOEngine.Resources
{
    internal interface IAssetLoader:IReference
    {
        void LoadAssetAsync();

        ELoaderStatus GetLoaderStatus();

        int GetPriority();

        void Pause();

    }
}