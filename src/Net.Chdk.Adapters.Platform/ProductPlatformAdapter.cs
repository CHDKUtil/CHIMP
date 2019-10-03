namespace Net.Chdk.Adapters.Platform
{
    public abstract class ProductPlatformAdapter : IProductPlatformAdapter
    {
        public abstract string ProductName { get; }

        public abstract string NormalizePlatform(string key);
    }
}
