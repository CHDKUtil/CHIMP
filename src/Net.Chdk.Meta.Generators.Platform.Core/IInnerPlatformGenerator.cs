namespace Net.Chdk.Meta.Generators.Platform
{
    public interface IInnerPlatformGenerator
    {
        string GetPlatform(uint modelId, string[] models);
    }
}
