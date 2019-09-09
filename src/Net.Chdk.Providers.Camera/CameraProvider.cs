using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Camera
{
    sealed class CameraProvider : ProviderResolver<IProductCameraProvider>, ICameraProvider
    {
        private IProductProvider ProductProvider { get; }

        public CameraProvider(IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ProductProvider = productProvider;
        }

        public CameraModelsInfo GetCameraModels(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo)
        {
            var productName = productInfo?.Name;
            return GetProvider(productName)?
                .GetCameraModels(cameraInfo);
        }

        public CameraModelsInfo GetCameraModels(CameraInfo cameraInfo)
        {
            return Providers.Values
                .Select(p => p.GetCameraModels(cameraInfo))
                .FirstOrDefault(c => c != null);
        }

        public SoftwareCameraInfo GetCamera(string productName, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo)
        {
            return GetProvider(productName)?
                .GetCamera(cameraInfo, cameraModelInfo);
        }

        public SoftwareEncodingInfo GetEncoding(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo)
        {
            var productName = productInfo?.Name;
            return GetProvider(productName)?
                .GetEncoding(cameraInfo);
        }

        public AltInfo GetAlt(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo)
        {
            var productName = productInfo?.Name;
            return GetProvider(productName)?
                .GetAlt(cameraInfo);
        }

        protected override IEnumerable<string> GetNames()
        {
            return ProductProvider.GetProductNames();
        }

        protected override IProductCameraProvider CreateProvider(string productName)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            switch (categoryName)
            {
                case "EOS":
                    return new EosProductCameraProvider(productName, LoggerFactory);
                case "PS":
                    return new PsProductCameraProvider(productName, LoggerFactory);
                default:
                    throw new InvalidOperationException($"Unknown category: {categoryName}");
            }
        }
    }
}
