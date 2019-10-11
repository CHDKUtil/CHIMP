using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Substitute
{
    sealed class SubstituteProvider : ProviderResolver<ICategorySubstituteProvider>, ISubstituteProvider
    {
        private IPlatformGenerator PlatformGenerator { get; }
        private IFirmwareProvider FirmwareProvider { get; }
        private ILogger Logger { get; }

        public SubstituteProvider(IPlatformGenerator platformGenerator, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PlatformGenerator = platformGenerator;
            FirmwareProvider = firmwareProvider;
            Logger = loggerFactory.CreateLogger<SubstituteProvider>();
        }

        public IDictionary<string, object>? GetSubstitutes(CameraInfo camera, CameraModelInfo cameraModel)
        {
            var categoryName = FirmwareProvider.GetCategoryName(camera);
            if (categoryName == null)
                return null;

            return GetProvider(categoryName)?
                .GetSubstitutes(camera, cameraModel)
                ?? GetDefaultSubstitutes(camera, cameraModel);
        }

        private IDictionary<string, object>? GetDefaultSubstitutes(CameraInfo camera, CameraModelInfo cameraModel)
        {
            var name = cameraModel.Names[0];
            if (name == null)
                return null;

            return new Dictionary<string, object>
            {
                ["model"] = name,
                ["platforms"] = GetSupportedPlatforms()
            };
        }

        private IEnumerable<string> GetSupportedPlatforms()
        {
            return Providers.Values
                .SelectMany(p => p.GetSupportedPlatforms());
        }

        protected override IEnumerable<string> GetNames()
        {
            return new[] { "EOS", "PS" };
        }

        protected override ICategorySubstituteProvider CreateProvider(string categoryName)
        {
            return new CategorySubstituteProvider(PlatformGenerator, FirmwareProvider, categoryName, Logger);
        }
    }
}
