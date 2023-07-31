
namespace HOEngine.Resources
{
    /// <summary>
    /// 资源接口
    /// </summary>
    public interface IProjectResource
    {
        /// <summary>
        /// 资源是否存在
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        bool HasAsset(string assetName);
    }
}

