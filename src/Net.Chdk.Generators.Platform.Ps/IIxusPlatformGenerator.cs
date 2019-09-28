namespace Net.Chdk.Generators.Platform.Ps
{
    interface IIxusPlatformGenerator
    {
        string? Generate(uint modelId, string source);
    }
}
