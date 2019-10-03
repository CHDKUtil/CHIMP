using Net.Chdk.Adapters.Platform;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraPlatformProvider : ICameraPlatformProvider
    {
        private IPlatformAdapter PlatformAdapter { get; }

        public CameraPlatformProvider(IPlatformAdapter platformAdapter)
        {
            PlatformAdapter = platformAdapter;
        }

        public PlatformData GetPlatform(string key, IDictionary<string, PlatformData> platform, string productName)
        {
            var key2 = PlatformAdapter.NormalizePlatform(productName, key);
            if (!platform.TryGetValue(key2, out PlatformData value))
                throw new InvalidOperationException($"{key} missing from platforms");
            return value;
        }

        public TreePlatformData GetTree(string key, IDictionary<string, TreePlatformData> tree, string productName)
        {
            if (!tree.TryGetValue(key, out TreePlatformData value))
                throw new InvalidOperationException($"{key} missing from platforms");
            return value;
        }
    }
}
