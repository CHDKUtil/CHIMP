using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Camera.Sdm
{
    sealed class SdmCameraPlatformProvider : ProductCameraPlatformProvider
    {
        public override string ProductName => "SDM";

        protected override T TryGetValue<T>(IDictionary<string, T> values, string platform)
        {
            return values.SingleOrDefault(kvp => IsMatch(kvp.Key, platform)).Value;
        }

        private static bool IsMatch(string key, string platform)
        {
            return GetPlatform(key).Equals(platform, StringComparison.Ordinal);
        }

        private static string GetPlatform(string platform)
        {
            return platform.StartsWith("ixus", StringComparison.Ordinal)
                ? platform.Split('_')[0]
                : platform;
        }
    }
}
