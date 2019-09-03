using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers
{
    sealed class CameraMetaProvider : SingleProductProvider<IProductCameraMetaProvider>, ICameraMetaProvider
    {
        public CameraMetaProvider(IEnumerable<IProductCameraMetaProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public CameraInfo GetCamera(string productName, string fileName)
        {
            return GetInnerProvider(productName).GetCamera(fileName);
        }
    }
}
