using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    abstract class InstallActionProvider<TAction> : ActionProvider
        where TAction : InstallActionBase
    {
        protected ISourceProvider SourceProvider { get; }
        protected ICameraModelProvider CameraProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        protected InstallActionProvider(MainViewModel mainViewModel, ISourceProvider sourceProvider, ICameraModelProvider cameraProvider, IFirmwareProvider firmwareProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            SourceProvider = sourceProvider;
            CameraProvider = cameraProvider;
            FirmwareProvider = firmwareProvider;

            category = new Lazy<CategoryInfo>(GetCategory);
            categoryName = new Lazy<string>(GetCategoryName);
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
            return SourceProvider.GetSources(Category);
        }

        private IAction CreateAction(ProductSource productSource)
        {
            var cameraModel = CameraProvider.GetCameraModel(CameraViewModel.Info, CameraViewModel.SelectedItem.Model);
            if (cameraModel == null)
                return null;
            return CreateAction(cameraModel?.Camera, cameraModel?.Model, productSource);
        }

        protected IAction CreateAction(SoftwareCameraInfo camera, SoftwareModelInfo model, ProductSource productSource)
        {
            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            var software = new SoftwareInfo
            {
                Category = Category,
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

        #region Category

        private readonly Lazy<CategoryInfo> category;
        private CategoryInfo Category => category.Value;
        private CategoryInfo GetCategory()
        {
            return new CategoryInfo
            {
                Name = CategoryName
            };
        }

        #endregion

        #region CategoryName

        private readonly Lazy<string> categoryName;
        protected virtual string CategoryName => categoryName.Value;
        private string GetCategoryName()
        {
            return FirmwareProvider.GetCategoryName(CameraViewModel?.Info);
        }

        #endregion
    }
}
