namespace HOEngine.Editor.PackResources
{
    internal enum PackType
    {
        //所有文件一个Bundle
        One, 
        PerFile,
        //文件夹一个Bundle
        PerDir,
        //场景
        Scene,
        //shader
        Shader,
    }
}