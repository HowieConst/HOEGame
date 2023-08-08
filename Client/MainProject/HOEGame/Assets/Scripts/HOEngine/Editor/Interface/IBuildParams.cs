using UnityEditor;
namespace HOEngine.Editor
{
    public interface IBuildParameters
    {
        BuildTarget Target { get; }
    }
}