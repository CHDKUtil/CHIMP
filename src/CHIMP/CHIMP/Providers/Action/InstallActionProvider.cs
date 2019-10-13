using Chimp.Actions;
using Chimp.Providers.Action.Install;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    sealed class InstallActionProvider : InstallActionProvider<InstallAction>
    {
        public InstallActionProvider(MainViewModel mainViewModel, ICameraModelProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, cameraProvider, serviceActivator)
        {
        }

        protected override IEnumerable<ProductSource> GetSources(IActionCreator creator, SoftwareProductInfo product)
        {
            var infoProduct = SoftwareViewModel.SelectedItem?.Info.Product;
            var infoSources = infoProduct == null
                ? Enumerable.Empty<ProductSource>()
                : creator.GetSources(infoProduct);
            return creator.GetSources(product)
                .Except(infoSources);
        }
    }
}
