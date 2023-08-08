using Sirenix.OdinInspector;

namespace HOEngine.Editor.PackResources
{
    public enum EPackType
    {
        //所有文件一个Bundle
        [LabelText("整个bundle")]
        One, 
        [LabelText("每个文件一个bundle")]
        PerFile,
        //文件夹一个Bundle
        [LabelText("每个文件夹一个bundle")]
        PerDir,
        [LabelText("场景打包")]
        //场景
        Scene,
        [LabelText("Shader 打包")]
        //shader
        Shader,
    }

    public enum EPackContent
    {
        [LabelText("打包UI")]
        PackUI,
        [LabelText("打包场景")]
        PackScene,
        [LabelText("打包材质")]
        PackMat,
        [LabelText("打包Act")]
        PackAct,
        [LabelText("打包Shader")]
        PackShader,
    }
}