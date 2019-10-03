using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    sealed class SubstituteProvider : ProviderResolver<ICategorySubstituteProvider>, ISubstituteProvider
    {
        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }
        private ILogger Logger { get; }

        public SubstituteProvider(IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PlatformProvider = platformProvider;
            FirmwareProvider = firmwareProvider;
            Logger = loggerFactory.CreateLogger<SubstituteProvider>();
        }

        public IDictionary<string, string>? GetSubstitutes(CameraInfo camera, CameraModelInfo cameraModel)
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
            return new CategorySubstituteProvider(categoryName, PlatformProvider, FirmwareProvider, Logger);
        }
    }
}
