using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class ClearOverlaysActionProvider : ActionProvider
    {
        private const string ProductName = "clear_overlays";

        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public ClearOverlaysActionProvider(MainViewModel mainViewModel, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            PlatformProvider = platformProvider;
            FirmwareProvider = firmwareProvider;
        }

        public override IEnumerable<IAction> GetActions()
        {
            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || (card?.Bootable != null && card?.Bootable != "SCRIPT"))
                yield break;

            var camera = GetCamera();
            var model = GetModel();
            var productSource = GetProductSource();

            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            if (softwareInfo?.Product?.Name == ProductName)
                yield return CreateAction<UpdateAction>(camera, model, productSource);
            else
                yield return CreateAction<InstallAction>(camera, model, productSource);
        }

        private TAction CreateAction<TAction>(SoftwareCameraInfo camera, SoftwareModelInfo model, ProductSource productSource)
            where TAction : IAction
        {
            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            var software = new SoftwareInfo
            {
                Product = GetProduct(),
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

        private ProductSource GetProductSource()
        {
            return new ProductSource(ProductName, ProductName, new SoftwareSourceInfo { Name = ProductName });
        }

        private SoftwareProductInfo GetProduct()
        {
            return new SoftwareProductInfo { Name = ProductName };
        }

        private SoftwareCameraInfo GetCamera()
        {
            var camera = CameraViewModel?.Info;
            var model = CameraViewModel?.SelectedItem?.Model;
            var categoryName = FirmwareProvider.GetCategoryName(camera);
            return new SoftwareCameraInfo
            {
                Platform = PlatformProvider.GetPlatform(camera, model, categoryName),
                Revision = FirmwareProvider.GetFirmwareRevision(camera, categoryName)
            };
        }

        private SoftwareModelInfo GetModel()
        {
            var camera = CameraViewModel?.Info;
            return new SoftwareModelInfo
            {
                Id = camera.Canon.ModelId,
                Name = camera.Base.Model
            };
        }
    }
}
