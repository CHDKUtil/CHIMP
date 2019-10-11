using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class ClearOverlaysActionProvider : ActionProvider
    {
        private const string ProductName = "clear_overlays";

        private ICameraModelProvider CameraProvider { get; }

        public ClearOverlaysActionProvider(MainViewModel mainViewModel, ICameraModelProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            CameraProvider = cameraProvider;
        }

        public override IEnumerable<IAction> GetActions()
        {
            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || (card?.Bootable != null && card?.Bootable != "SCRIPT"))
                yield break;

            var cameraModel = CameraProvider.GetCameraModel(CameraViewModel?.Info, CameraViewModel?.SelectedItem?.Model);
            if (cameraModel == null)
                yield break;

            (var camera, var model) = cameraModel.Value;
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
    }
}
