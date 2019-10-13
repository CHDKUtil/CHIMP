using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Chimp.Providers.Action.Install
{
    interface IActionCreator
    {
        IEnumerable<SoftwareProductInfo> GetProducts(CardItemViewModel card, CameraInfo camera);
        IEnumerable<ProductSource> GetSources(SoftwareProductInfo product);
        TAction CreateAction<TAction>(ProductSource productSource)
            where TAction : InstallActionBase;
    }
}
