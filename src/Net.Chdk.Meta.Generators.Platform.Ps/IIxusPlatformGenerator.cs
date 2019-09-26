namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    interface IIxusPlatformGenerator : IPlatformGenerator
    {
        string? Generate(uint modelId, string source);
    }
}
