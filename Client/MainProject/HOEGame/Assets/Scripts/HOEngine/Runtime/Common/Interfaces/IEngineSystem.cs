namespace HOEngine
{
    internal interface IEngineManager
    {
        void Init(params object[] param);

        void Update();

        void Clear();

        void Dispose();
    }
}