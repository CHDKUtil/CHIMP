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
    sealed class UpdateActionProvider : InstallActionProvider<UpdateAction>
    {
        public UpdateActionProvider(MainViewModel mainViewModel, ICameraModelProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, cameraProvider, serviceActivator)
        {
        }

        protected override IEnumerable<IAction> GetActions(IActionCreator creator, SoftwareProductInfo product)
        {
            var infoProduct = SoftwareViewModel?.SelectedItem?.Info.Product;
            return infoProduct?.Name == product.Name
                ? base.GetActions(creator, infoProduct)
                : Enumerable.Empty<IAction>();
        }

        protected override IEnumerable<ProductSource> GetSources(IActionCreator creator, SoftwareProductInfo product)
        {
            return creator.GetSources(product);
        }
    }
}
