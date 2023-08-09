namespace HOEngine.Editor
{
    public interface IBuildStep
    {
        string BuildStepName { get; }

        ReturnCode Run(IBuildContent content);
    }
}