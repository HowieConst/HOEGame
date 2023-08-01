namespace HOEngine.Resources
{
    /// <summary>
    /// 资源模式
    /// </summary>
    public enum EResourceMode
    {
        None = 0,
        
        /// <summary>
        /// Editor 模式
        /// </summary>
        Editor = 1,
        
        /// <summary>
        /// Bundle 模式
        /// </summary>
        AssetBundle = 2,
    }

    /// <summary>
    /// 加载优先级
    /// </summary>
    public enum ELoadPriority
    {
        PostLoad = -1000,
        Default = 0,
        Prior = 100,
        HighPrior = 1000,
        MostPrior = 10000,
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum EAssetType
    {
        /// <summary>
        /// Unity 对象
        /// </summary>
        Object = 0,
        
        /// <summary>
        /// 预设
        /// </summary>
        Prefab ,
        
        
        /// <summary>
        /// 图片
        /// </summary>
        Texture,
        
        /// <summary>
        /// 精灵
        /// </summary>
        Sprite,
        
        /// <summary>
        /// 网格
        /// </summary>
        Mesh,
        
        /// <summary>
        /// 场景
        /// </summary>
        Scene,
        
        /// <summary>
        /// 动画片段
        /// </summary>
        AnimationClip,
        
        /// <summary>
        /// 字体
        /// </summary>
        Font,
        
        /// <summary>
        /// 材质球
        /// </summary>
        Material,
        
        /// <summary>
        /// shader
        /// </summary>
        Shader ,
        
        /// <summary>
        /// 文本资源
        /// </summary>
        TextAsset ,
        
        /// <summary>
        /// 图集
        /// </summary>
        SpriteAtlas,
        
        /// <summary>
        /// 序列化脚本
        /// </summary>
        ScriptableObject,
        
        
        
        
    }

    public enum ELoaderStatus
    {
        None = 0,
        //等待加载
        Wait = 1,
        //正在加载Bundle
        LoadBundleDependencyFinish,
     
        LoadBunldeFinish,
        //加载资源
        LoadAsset,
        //加载资源完成
        LoadFinish,
        //卸载
        UnLoad,
        
    }
}

