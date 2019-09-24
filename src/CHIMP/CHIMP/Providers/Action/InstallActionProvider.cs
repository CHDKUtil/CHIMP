using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    sealed class InstallActionProvider : InstallActionProvider<InstallAction>
    {
        private IProductProvider ProductProvider { get; }

        public InstallActionProvider(MainViewModel mainViewModel, IProductProvider productProvider, ISourceProvider sourceProvider, ICameraProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, sourceProvider, cameraProvider, serviceActivator)
        {
            ProductProvider = productProvider;
        }

        protected override IEnumerable<IAction> GetActions(SoftwareProductInfo? product)
        {
            if (product is null)
                return base.GetActions(product);
            var camera = CameraProvider.GetCamera(product.Name, CameraViewModel?.Info, CameraViewModel?.SelectedItem?.Model);
            if (camera is null)
                return Enumerable.Empty<IAction>();
            return GetSources(product)
                .Select(s => CreateAction(camera, s));
        }

        protected override IEnumerable<ProductSource> GetSources(SoftwareProductInfo? product)
        {
            var infoProduct = SoftwareViewModel?.SelectedItem?.Info?.Product;
            var infoSources = infoProduct == null
                ? Enumerable.Empty<ProductSource>()
                : base.GetSources(infoProduct);
            return base.GetSources(product)
                .Except(infoSources);
        }

        protected override IEnumerable<SoftwareProductInfo> GetProducts()
        {
            return ProductProvider.GetProductNames()
                .Where(IsValidProduct)
                .Select(CreateProduct);
        }

        private bool IsValidProduct(string productName)
        {
            return ProductProvider.GetCategoryName(productName).Equals(CategoryName);
        }

        private static SoftwareProductInfo CreateProduct(string productName)
        {
            return new SoftwareProductInfo
            {
                Name = productName,
            };
        }
    }
}
