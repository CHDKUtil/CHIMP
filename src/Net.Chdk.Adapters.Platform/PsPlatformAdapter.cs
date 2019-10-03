using System.Collections.Generic;

namespace Net.Chdk.Adapters.Platform
{
    public abstract class PsPlatformAdapter : ProductPlatformAdapter
    {
        private static readonly Dictionary<string, string> Platforms = new Dictionary<string, string>
        {
            ["ixusw"] = "ixuswireless",
            ["n_facebook"] = "nfacebook",
        };

        public override string NormalizePlatform(string platform)
        {
            if (Platforms.TryGetValue(platform, out string platform2))
                return platform2;

            return platform
                .TrimEnd("is")
                .TrimEnd("hs");
        }
    }
}
