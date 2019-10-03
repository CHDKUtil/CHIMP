namespace Net.Chdk.Adapters.Platform
{
    public interface IPlatformAdapter
    {
        string NormalizePlatform(string productName, string platform);
    }
}
