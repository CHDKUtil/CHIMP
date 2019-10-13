using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    sealed class InstallActionProvider : InstallActionProvider<InstallAction>
    {
        private const string ScriptCategoryName = "SCRIPT";

        private IProductProvider ProductProvider { get; }

        public InstallActionProvider(MainViewModel mainViewModel, IProductProvider productProvider, ISourceProvider sourceProvider, ICameraModelProvider cameraProvider, IFirmwareProvider firmwareProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, sourceProvider, cameraProvider, firmwareProvider, serviceActivator)
        {
            ProductProvider = productProvider;
        }

        protected override IEnumerable<IAction> GetActions(SoftwareProductInfo product)
        {
            if (product == null)
                return base.GetActions(product);
            var cameraModel = CameraProvider.GetCameraModel(CameraViewModel?.Info, CameraViewModel?.SelectedItem?.Model);
            if (cameraModel == null)
                return Enumerable.Empty<IAction>();
            return GetSources(product)
                .Select(s => CreateAction(cameraModel?.Camera, cameraModel?.Model, s));
        }

        protected override IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            var infoProduct = SoftwareViewModel.SelectedItem?.Info.Product;
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
            var categoryName = ProductProvider.GetCategoryName(productName);
            if (categoryName == CategoryName)
                return true;
            if (categoryName == ScriptCategoryName)
                return IsScriptInstallable();
            return false;
        }

        private bool IsScriptInstallable()
        {
            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || (card?.Bootable != null && card?.Bootable != ScriptCategoryName))
                return false;
            return true;
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
