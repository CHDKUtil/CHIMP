using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using System.Collections.Generic;

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
                .GetSubstitutes(camera, cameraModel);
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
