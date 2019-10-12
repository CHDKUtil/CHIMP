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
    sealed class InstallScriptActionProvider : InstallActionProvider
    {
        public InstallScriptActionProvider(MainViewModel mainViewModel, IProductProvider productProvider, ISourceProvider sourceProvider, ICameraModelProvider cameraProvider, IFirmwareProvider firmwareProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, productProvider, sourceProvider, cameraProvider, firmwareProvider, serviceActivator)
        {
        }

        protected override IEnumerable<IAction> GetActions(SoftwareProductInfo product)
        {
            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || (card?.Bootable != null && card?.Bootable != CategoryName))
                return Enumerable.Empty<IAction>();

            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            if (softwareInfo?.Product?.Name == product.Name)
                return Enumerable.Empty<IAction>();

            return base.GetActions(product);
        }

        protected override string CategoryName => "SCRIPT";
    }
}
