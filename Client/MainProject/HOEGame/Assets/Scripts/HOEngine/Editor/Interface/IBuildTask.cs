namespace HOEngine.Editor
{
    public interface IBuildTask
    {
        string BuildStepName { get; }
        
        IBuildParameters getBuildParameters();
    }
}