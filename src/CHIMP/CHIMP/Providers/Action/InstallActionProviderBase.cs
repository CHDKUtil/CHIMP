using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    abstract class InstallActionProvider<TAction> : ActionProvider
        where TAction : InstallActionBase
    {
        protected ISourceProvider SourceProvider { get; }
        protected ICameraModelProvider CameraProvider { get; }
        protected string CategoryName { get; }

        protected InstallActionProvider(MainViewModel mainViewModel, ISourceProvider sourceProvider, ICameraModelProvider cameraProvider, IFirmwareProvider firmwareProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            SourceProvider = sourceProvider;
            CameraProvider = cameraProvider;
            CategoryName = firmwareProvider.GetCategoryName(CameraViewModel?.Info);
        }

        public override IEnumerable<IAction> GetActions()
        {
            return GetProducts()
                .SelectMany(GetActions)
                .Where(a => a != null);
        }

        protected virtual IEnumerable<IAction> GetActions(SoftwareProductInfo product)
        {
            return GetSources(product)
                .Select(CreateAction);
        }

        protected abstract IEnumerable<SoftwareProductInfo> GetProducts();

        protected virtual IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            if (product?.Name != null)
                return SourceProvider.GetSources(product);
            var category = GetCategory();
            return SourceProvider.GetSources(category);
        }

        private IAction CreateAction(ProductSource productSource)
        {
            var cameraModel = CameraProvider.GetCameraModel(productSource.ProductName, CameraViewModel.Info, CameraViewModel.SelectedItem.Model);
            if (cameraModel == null)
                return null;
            return CreateAction(cameraModel?.Camera, cameraModel?.Model, productSource);
        }

        protected IAction CreateAction(SoftwareCameraInfo camera, SoftwareModelInfo model, ProductSource productSource)
        {
            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            var software = new SoftwareInfo
            {
                Product = softwareInfo?.Product,
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
    }
}
