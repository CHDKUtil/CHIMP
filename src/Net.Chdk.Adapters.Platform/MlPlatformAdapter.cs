namespace Net.Chdk.Adapters.Platform
{
    sealed class MlPlatformAdapter : EosPlatformAdapter
    {
        public override string ProductName => "ML";

        public override string NormalizePlatform(string platform)
        {
            if (platform == "5DC")
                return "5d";
            platform = platform.TrimStart("EOS");
            return base.NormalizePlatform(platform);
        }
    }
}
