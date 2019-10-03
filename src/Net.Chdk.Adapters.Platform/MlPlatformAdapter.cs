using System.Collections.Generic;

namespace Net.Chdk.Adapters.Platform
{
    sealed class MlPlatformAdapter : EosPlatformAdapter
    {
        private static readonly Dictionary<string, string> Platforms = new Dictionary<string, string>
        {
            ["5DC"] = "5D",
            ["EOSM"] = "M",
        };

        public override string ProductName => "ML";

        public override string NormalizePlatform(string platform)
        {
            platform = base.NormalizePlatform(platform);
            if (Platforms.TryGetValue(platform, out string platform2))
                return platform2;
            return platform;
        }
    }
}
