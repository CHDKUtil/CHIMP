namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    interface IIxusPlatformGenerator
    {
        string? Generate(uint modelId, string source);
    }
}
