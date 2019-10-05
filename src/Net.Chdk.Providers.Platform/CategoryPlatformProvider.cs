using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Platform;

namespace Net.Chdk.Providers.Platform
{
    abstract class CategoryPlatformProvider : DataProvider<Dictionary<string, CameraModel[]>>, IInnerPlatformProvider
    {
        protected CategoryPlatformProvider(ILogger logger)
            : base(logger)
        {
            platforms = new Lazy<Dictionary<string, PlatformData>>(GetPlatforms);
        }

        public IEnumerable<KeyValuePair<string, PlatformData>>? GetPlatforms(uint modelId)
        {
            var key = $"0x{modelId:x07}";
            Data.TryGetValue(key, out CameraModel[]? models);
            return models?
                .Select(m => GetKeyValuePair(key, m));
        }

        public PlatformData? GetPlatform(string platform)
        {
            Platforms.TryGetValue(platform, out PlatformData? platformData);
            return platformData;
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, "platforms.json");
        }

        protected abstract string CategoryName { get; }

        private static KeyValuePair<string, PlatformData> GetKeyValuePair(string modelId, CameraModel model)
        {
            return new KeyValuePair<string, PlatformData>(model.Platform!, GetPlatform(modelId, model));
        }

        private static PlatformData GetPlatform(string modelId, CameraModel model)
        {
            return new PlatformData { ModelId = modelId, Names = model.Names };
        }

        #region ReversePlatforms

        private readonly Lazy<Dictionary<string, PlatformData>> platforms;

        private Dictionary<string, PlatformData> Platforms => platforms.Value;

        private Dictionary<string, PlatformData> GetPlatforms()
        {
            return Data
                .SelectMany(kvp => kvp.Value.Select(model => GetKeyValuePair(kvp.Key, model)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        #endregion
    }
}
