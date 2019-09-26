namespace Net.Chdk.Meta.Generators.Platform
{
    public interface IPlatformGenerator
    {
        string? GetPlatform(uint modelId, string[] models, bool isCanon = false);
    }
}
