namespace Net.Chdk.Adapters.Platform
{
    public interface IProductPlatformAdapter
    {
        string NormalizePlatform(string platform);
        string ProductName { get; }
    }
}
