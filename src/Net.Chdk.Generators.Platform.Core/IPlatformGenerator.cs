namespace Net.Chdk.Generators.Platform
{
    public interface IPlatformGenerator
    {
        string? GetPlatform(uint modelId, string[] models, bool isCanon = false);
    }
}
