using Net.Chdk.Meta.Model.Camera;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraBootProvider : SingleProductProvider<IProductCameraBootProvider>, ICameraBootProvider
    {
        public CameraBootProvider(IEnumerable<IProductCameraBootProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public BootData GetBoot(uint modelId, string productName)
        {
            return GetInnerProvider(productName).GetBoot(modelId);
        }
    }
}
