using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Providers.CameraModel
{
    sealed class CameraModelProvider : ProviderResolver<IProductCameraModelProvider>, ICameraModelProvider
    {
        private IProductProvider ProductProvider { get; }

        public CameraModelProvider(IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ProductProvider = productProvider;
        }

        public SoftwareEncodingInfo? GetEncoding(SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return GetProvider(product.Name)?
                .GetEncoding(camera);
        }

        public string? GetAltButton(SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return GetProvider(product.Name)?
                .GetAltButton(camera);
        }

        public string? GetCardType(SoftwareProductInfo product, CameraInfo camera)
        {
            return GetProvider(product.Name)?
                .GetCardType(camera);
        }

        public string? GetCardSubtype(SoftwareProductInfo product, CameraInfo camera)
        {
            return GetProvider(product.Name)?
                .GetCardSubtype(camera);
        }

        public bool? IsMultiPartition(SoftwareProductInfo product, CameraInfo camera)
        {
            return GetProvider(product.Name)?
                .IsMultiPartition(camera);
        }

        public string? GetBootFileSystem(SoftwareProductInfo product, CameraInfo camera)
        {
            return GetProvider(product.Name)?
                .GetBootFileSystem(camera);
        }

        protected override IEnumerable<string> GetNames()
        {
            return ProductProvider.GetProductNames();
        }

        protected override IProductCameraModelProvider? CreateProvider(string productName)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            return categoryName switch
            {
                "EOS" => new EosCameraModelProvider(productName, LoggerFactory),
                "PS" => new PsCameraModelProvider(productName, LoggerFactory),
                "SCRIPT" => null,
                _ => throw new InvalidOperationException($"Unknown category: {categoryName}"),
            };
        }
    }
}
