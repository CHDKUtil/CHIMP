using Chimp.ViewModels;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action.Install
{
    sealed class ScriptActionCreator : ActionCreatorBase
    {
        public ScriptActionCreator(SoftwareProductInfo product, SoftwareSourceInfo source, SoftwareCameraInfo camera, SoftwareModelInfo model, IProductProvider productProvider, IServiceActivator serviceActivator)
            : base(product, source, camera, model, productProvider, serviceActivator)
        {
        }

        public override IEnumerable<SoftwareProductInfo> GetProducts(CardItemViewModel card, CameraInfo camera)
        {
            if (card?.Bootable != null && card?.Bootable != CategoryName)
                return Enumerable.Empty<SoftwareProductInfo>();
            return GetProducts();
        }

        public override IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            var productName = product?.Name;
            if (productName != null)
                yield return new ProductSource(productName, productName, new SoftwareSourceInfo { Name = productName });
        }

        protected override string CategoryName => "SCRIPT";
    }
}
