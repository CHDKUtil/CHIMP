using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    sealed class SubstituteProvider : ProviderResolver<ICategorySubstituteProvider>, ISubstituteProvider
    {
        private IPlatformGenerator PlatformGenerator { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public SubstituteProvider(IPlatformGenerator platformGenerator, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PlatformGenerator = platformGenerator;
            FirmwareProvider = firmwareProvider;
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

        protected override ICategorySubstituteProvider CreateProvider(string categoryName) => categoryName switch
        {
            "EOS" => new EosSubstituteProvider(PlatformGenerator, FirmwareProvider, LoggerFactory),
            "PS" => new PsSubstituteProvider(PlatformGenerator, FirmwareProvider, LoggerFactory),
            _ => throw new InvalidOperationException($"Unknown category: {categoryName}"),
        };
    }
}
