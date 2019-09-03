using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraModelValidator : SingleProductProvider<IProductCameraModelValidator>, ICameraModelValidator
    {
        public CameraModelValidator(IEnumerable<IProductCameraModelValidator> innerValidators)
            : base(innerValidators)
        {
        }

        public void Validate(string platform, ListPlatformData list, TreePlatformData tree, string productName)
        {
            GetInnerProvider(productName).Validate(platform, list, tree);
        }
    }
}
