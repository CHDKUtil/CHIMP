namespace Net.Chdk.Adapters.Platform
{
    public abstract class EosPlatformAdapter : ProductPlatformAdapter
    {
        public override string NormalizePlatform(string platform)
        {
            return platform.ToUpper();
        }
    }
}
