using Microsoft.Extensions.Logging;
using Net.Chdk.Adapters.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.CameraModel
{
    sealed class CameraModelProvider : ProviderResolver<ICategoryCameraModelProvider>, ICameraModelProvider
    {
        private IPlatformAdapter PlatformAdapter { get; }
        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public CameraModelProvider(IPlatformAdapter platformAdapter, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PlatformAdapter = platformAdapter;
            PlatformProvider = platformProvider;
            FirmwareProvider = firmwareProvider;
        }

        public (CameraInfo, CameraModelInfo[])? GetCameraModels(SoftwareProductInfo? product, SoftwareCameraInfo? cameraInfo)
        {
            return Providers.Values
                .Select(p => p.GetCameraModels(product, cameraInfo))
                .FirstOrDefault(c => c != null);
        }

        public (CameraInfo, CameraModelInfo[])? GetCameraModels(SoftwareCameraInfo? cameraInfo, SoftwareModelInfo? cameraModelInfo)
        {
            return Providers.Values
                .Select(p => p.GetCameraModels(cameraInfo, cameraModelInfo))
                .FirstOrDefault(c => c != null);
        }

        public (CameraInfo, CameraModelInfo[])? GetCameraModels(CameraInfo cameraInfo)
        {
            var categoryName = FirmwareProvider.GetCategoryName(cameraInfo);
            return GetProvider(categoryName)?
                .GetCameraModels(cameraInfo);
        }

        public (SoftwareCameraInfo, SoftwareModelInfo)? GetCameraModel(CameraInfo cameraInfo, CameraModelInfo cameraModelInfo)
        {
            return Providers.Values
                .Select(p => p.GetCameraModel(cameraInfo, cameraModelInfo))
                .FirstOrDefault(r => r != null);
        }

        protected override IEnumerable<string> GetNames()
        {
            return new[] { "EOS", "PS" };
        }

        protected override ICategoryCameraModelProvider CreateProvider(string categoryName)
        {
            switch (categoryName)
            {
                case "EOS":
                    return new EosCameraModelProvider(PlatformAdapter, PlatformProvider, FirmwareProvider);
                case "PS":
                    return new PsCameraModelProvider(PlatformAdapter, PlatformProvider, FirmwareProvider);
                default:
                    throw new InvalidOperationException($"Unknown category: {categoryName}");
            }
        }
    }
}
