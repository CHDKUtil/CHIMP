using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Platform
{
    sealed class PlatformProvider : ProviderResolver<IInnerPlatformProvider>, IPlatformProvider
    {
        public PlatformProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public string? GetPlatform(CameraInfo camera, CameraModelInfo cameraModel, string categoryName)
        {
            var model = cameraModel?.Names[0];
            if (model == null)
                return null;

            var kvps = DoGetPlatforms(camera, categoryName);
            return kvps?
                .SingleOrDefault(kvp => model == kvp.Value.Names?[0])
                .Key;
        }

        public PlatformData? GetPlatform(string platform, string categoryName)
        {
            if (platform == null)
                return null;

            return GetProvider(categoryName)?
                .GetPlatform(platform);
        }

        public PlatformData[]? GetPlatforms(CameraInfo camera, string categoryName)
        {
            var kvps = DoGetPlatforms(camera, categoryName);
            return kvps?
                .Select(kvp => kvp.Value)
                .ToArray();
        }

        private IEnumerable<KeyValuePair<string, PlatformData>>? DoGetPlatforms(CameraInfo camera, string categoryName)
        {
            var modelId = camera?.Canon?.ModelId;
            if (modelId == null || modelId.Value == 0)
                return null;

            return GetProvider(categoryName)?
                .GetPlatforms(modelId.Value);
        }

        protected override IEnumerable<string> GetNames()
        {
            return new[] { "EOS", "PS" };
        }

        protected override IInnerPlatformProvider CreateProvider(string categoryName)
        {
            return categoryName switch
            {
                "EOS" => new EosPlatformProvider(LoggerFactory),
                "PS" => new PsPlatformProvider(LoggerFactory),
                _ => throw new InvalidOperationException($"Unknown category: {categoryName}"),
            };
        }
    }
}
