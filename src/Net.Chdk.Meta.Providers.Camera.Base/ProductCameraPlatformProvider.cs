using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class ProductCameraPlatformProvider : IProductCameraPlatformProvider
    {
        public PlatformData GetPlatform(string key, IDictionary<string, PlatformData> platforms)
        {
            var value = TryGetValue(platforms, key);
            if (value == null)
                throw new InvalidOperationException($"{key} missing from platforms");
            return value;
        }

        public TreePlatformData GetTree(string key, IDictionary<string, TreePlatformData> tree)
        {
            var value = TryGetValue(tree, key);
            if (value == null)
                throw new InvalidOperationException($"{key} missing from tree");
            return value;
        }

        public abstract string ProductName { get; }

        protected virtual T TryGetValue<T>(IDictionary<string, T> values, string key)
            where T : class
        {
            values.TryGetValue(key, out T value);
            return value;
        }
    }
}
