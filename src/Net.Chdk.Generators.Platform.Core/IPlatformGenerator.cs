namespace Net.Chdk.Generators.Platform
{
    public interface IPlatformGenerator
    {
        string? GetPlatform(uint modelId, string[] models, string? category = null, bool isCanon = false);
    }
}
