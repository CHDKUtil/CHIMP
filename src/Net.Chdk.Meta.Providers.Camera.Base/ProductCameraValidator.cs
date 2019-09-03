using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class ProductCameraValidator : IProductCameraValidator
    {
        protected ILogger Logger { get; }

        protected ProductCameraValidator(ILogger logger)
        {
            Logger = logger;
        }

        public abstract string ProductName { get; }

        public void Validate(IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree)
        {
            foreach (var kvp in tree)
                Validate(kvp, list);
        }

        private void Validate(KeyValuePair<string, TreePlatformData> kvp, IDictionary<string, ListPlatformData> list)
        {
            var platform = kvp.Key;
            if (!list.ContainsKey(platform))
                OnListPlatformMissing(platform);
        }

        protected virtual void OnListPlatformMissing(string platform)
        {
            throw new InvalidOperationException($"{platform} missing from list");
        }
    }
}
