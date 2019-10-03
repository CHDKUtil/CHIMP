namespace Net.Chdk.Adapters.Platform
{
    sealed class ChdkPlatformAdapter : PsPlatformAdapter
    {
        public override string ProductName => "CHDK";

        public override string NormalizePlatform(string platform)
        {
            if (platform.StartsWith("ixus"))
                platform = platform.Split('_')[0];

            return base.NormalizePlatform(platform);
        }
    }
}
