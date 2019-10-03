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
            reversePlatforms = new Lazy<Dictionary<string, PlatformData>>(GetReversePlatforms);
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
            ReversePlatforms.TryGetValue(platform, out PlatformData? platformData);
            return platformData;
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, "platforms.json");
        }

        protected abstract string CategoryName { get; }

        private static KeyValuePair<string, PlatformData> GetKeyValuePair(string key, CameraModel model)
        {
            return new KeyValuePair<string, PlatformData>(model.Platform!, GetPlatform(key, model));
        }

        private static PlatformData GetPlatform(string key, CameraModel model)
        {
            return new PlatformData { ModelId = key, Names = model.Names };
        }

        #region ReversePlatforms

        private readonly Lazy<Dictionary<string, PlatformData>> reversePlatforms;

        private Dictionary<string, PlatformData> ReversePlatforms => reversePlatforms.Value;

        private Dictionary<string, PlatformData> GetReversePlatforms()
        {
            var platforms = new Dictionary<string, PlatformData>();
            foreach (var kvp in Data)
                foreach (var platform in kvp.Value)
                    if (platform.Platform != null)
                        platforms.Add(platform.Platform, new PlatformData { ModelId = kvp.Key, Names = platform.Names });
            return platforms;
        }

        #endregion
    }
}
