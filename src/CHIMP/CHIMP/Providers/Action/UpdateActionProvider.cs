using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class UpdateActionProvider : InstallActionProvider<UpdateAction>
    {
        public UpdateActionProvider(MainViewModel mainViewModel, ISourceProvider sourceProvider, ICameraModelProvider cameraProvider, IFirmwareProvider firmwareProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, sourceProvider, cameraProvider, firmwareProvider, serviceActivator)
        {
        }

        protected override IEnumerable<SoftwareProductInfo> GetProducts()
        {
            var product = SoftwareViewModel.SelectedItem?.Info.Product;
            if (product?.Name != null)
                yield return product;
        }
    }
}
