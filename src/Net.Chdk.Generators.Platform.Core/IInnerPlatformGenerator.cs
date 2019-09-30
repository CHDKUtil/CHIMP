namespace Net.Chdk.Generators.Platform
{
    public interface IInnerPlatformGenerator
    {
        string? GetPlatform(uint modelId, string[] models);
        string CategoryName { get; }
    }
}
