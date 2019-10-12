using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    sealed class InstallScriptActionProvider : ActionProvider
    {
        private const string CategoryName = "SCRIPT";

        private IProductProvider ProductProvider { get; }
        private ISourceProvider SourceProvider { get; }
        private ICameraModelProvider CameraProvider { get; }

        public InstallScriptActionProvider(MainViewModel mainViewModel, IProductProvider productProvider, ISourceProvider sourceProvider, ICameraModelProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            ProductProvider = productProvider;
            SourceProvider = sourceProvider;
            CameraProvider = cameraProvider;
        }

        public override IEnumerable<IAction> GetActions()
        {
            return GetProducts()
                .SelectMany(GetActions)
                .Where(a => a != null);
        }

        private IEnumerable<IAction> GetActions(SoftwareProductInfo product)
        {
            return GetSources(product)
                .Select(CreateAction);
        }

        private IAction CreateAction(ProductSource productSource)
        {
            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || (card?.Bootable != null && card?.Bootable != CategoryName))
                return null;

            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            if (softwareInfo?.Product?.Name == productSource.ProductName)
                return null;

            var cameraModel = CameraProvider.GetCameraModel(CameraViewModel?.Info, CameraViewModel?.SelectedItem?.Model);
            if (cameraModel == null)
                return null;

            (var camera, var model) = cameraModel.Value;
            return CreateAction<InstallAction>(camera, model, productSource);
        }

        private TAction CreateAction<TAction>(SoftwareCameraInfo camera, SoftwareModelInfo model, ProductSource productSource)
            where TAction : IAction
        {
            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            var software = new SoftwareInfo
            {
                Category = GetCategory(),
                Product = GetProduct(productSource),
                Source = softwareInfo?.Source,
                Camera = camera,
                Model = model,
            };
            var types = new[]
            {
                typeof(SoftwareInfo),
                typeof(ProductSource)
            };
            var values = new object[]
            {
                software,
                productSource
            };
            return ServiceActivator.Create<TAction>(types, values);
        }

        private CategoryInfo GetCategory()
        {
            return new CategoryInfo
            {
                Name = CategoryName
            };
        }

        private IEnumerable<SoftwareProductInfo> GetProducts()
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

        private static SoftwareProductInfo GetProduct(ProductSource productSource)
        {
            return CreateProduct(productSource.ProductName);
        }

        private IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            return SourceProvider.GetSources(product);
        }
    }
}
