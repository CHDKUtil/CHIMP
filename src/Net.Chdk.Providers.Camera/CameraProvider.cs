using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Camera
{
    sealed class CameraProvider : ProviderResolver<ICategoryCameraProvider>, ICameraProvider
    {
        private IProductProvider ProductProvider { get; }
        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public CameraProvider(IProductProvider productProvider, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ProductProvider = productProvider;
            PlatformProvider = platformProvider;
            FirmwareProvider = firmwareProvider;
        }

        public CameraModelsInfo? GetCameraModels(SoftwareProductInfo? _, SoftwareCameraInfo? cameraInfo)
        {
            return Providers.Values
                .Select(p => p.GetCameraModels(cameraInfo))
                .FirstOrDefault(c => c != null);
        }

        public SoftwareCameraInfo? GetCamera(string productName, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            return GetProvider(categoryName)?
                .GetCamera(cameraInfo, cameraModelInfo);
        }

        public CameraModelsInfo? GetCameraModels(CameraInfo cameraInfo)
        {
            var categoryName = FirmwareProvider.GetCategoryName(cameraInfo);
            return GetProvider(categoryName)?
                .GetCameraModels(cameraInfo);
        }

        protected override IEnumerable<string> GetNames()
        {
            return new[] { "EOS", "PS" };
        }

        protected override ICategoryCameraProvider CreateProvider(string categoryName)
        {
            switch (categoryName)
            {
                case "EOS":
                    return new EosCameraProvider(PlatformProvider, FirmwareProvider);
                case "PS":
                    return new PsCameraProvider(PlatformProvider, FirmwareProvider);
                default:
                    throw new InvalidOperationException($"Unknown category: {categoryName}");
            }
        }
    }
}
