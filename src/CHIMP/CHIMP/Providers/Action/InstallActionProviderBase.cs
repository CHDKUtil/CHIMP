using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    abstract class InstallActionProvider<TAction> : ActionProvider
        where TAction : InstallActionBase
    {
        protected ISourceProvider SourceProvider { get; }
        protected ICameraProvider CameraProvider { get; }
        protected string CategoryName { get; }

        public InstallActionProvider(MainViewModel mainViewModel, ISourceProvider sourceProvider, ICameraProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            SourceProvider = sourceProvider;
            CameraProvider = cameraProvider;
            CategoryName = CameraViewModel.Info.Canon.FirmwareRevision > 0
                ? "PS"
                : "EOS";
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
            var camera = CameraProvider.GetCameraModel(productSource.ProductName, CameraViewModel.Info, CameraViewModel.SelectedItem.Model);
            if (camera == null)
                return null;
            return CreateAction(camera, productSource);
        }

        protected IAction CreateAction((SoftwareCameraInfo Camera, SoftwareModelInfo Model)? cameraModel, ProductSource productSource)
        {
            var types = new[]
            {
                typeof(SoftwareCameraInfo),
                typeof(SoftwareModelInfo),
                typeof(ProductSource)
            };
            var values = new object[]
            {
                cameraModel?.Camera,
                cameraModel?.Model,
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
