using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraValidator : SingleProductProvider<IProductCameraValidator>, ICameraValidator
    {
        public CameraValidator(IEnumerable<IProductCameraValidator> innerValidators)
            : base(innerValidators)
        {
        }

        public void Validate(IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree, string productName)
        {
            GetInnerProvider(productName).Validate(list, tree);
        }
    }
}
