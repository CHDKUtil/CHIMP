using Microsoft.Extensions.Logging;
using Net.Chdk.Adapters.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.CameraModel
{
    sealed class CameraModelProvider : ProviderResolver<ICategoryCameraModelProvider>, ICameraModelProvider
    {
        private IProductProvider ProductProvider { get; }
        private IPlatformAdapter PlatformAdapter { get; }
        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public CameraModelProvider(IProductProvider productProvider, IPlatformAdapter platformAdapter, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ProductProvider = productProvider;
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

        public (SoftwareCameraInfo, SoftwareModelInfo)? GetCameraModel(string productName, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            return GetProvider(categoryName)?
                .GetCameraModel(cameraInfo, cameraModelInfo);
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
