using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class UpdateActionProvider : InstallActionProvider<UpdateAction>
    {
        public UpdateActionProvider(MainViewModel mainViewModel, ISourceProvider sourceProvider, ICameraProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, sourceProvider, cameraProvider, serviceActivator)
        {
        }

        protected override IEnumerable<SoftwareProductInfo> GetProducts()
        {
            var product = SoftwareViewModel?.SelectedItem?.Info?.Product;
            if (product != null)
                yield return product;
        }
    }
}
