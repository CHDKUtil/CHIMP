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
    abstract class InstallActionProvider<TAction> : ActionProvider
        where TAction : InstallActionBase
    {
        protected ICameraModelProvider CameraProvider { get; }

        protected InstallActionProvider(MainViewModel mainViewModel, ICameraModelProvider cameraProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            CameraProvider = cameraProvider;
        }

        public override IEnumerable<IAction> GetActions()
        {
            return GetCreators()
                .SelectMany(GetActions);
        }

        private IEnumerable<IActionCreator> GetCreators()
        {
            var cameraModel = CameraProvider.GetCameraModel(CameraViewModel?.Info, CameraViewModel?.SelectedItem?.Model);
            if (cameraModel == null)
                yield break;

            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            var types = new[]
            {
                typeof(SoftwareProductInfo),
                typeof(SoftwareSourceInfo),
                typeof(SoftwareCameraInfo),
                typeof(SoftwareModelInfo)
            };
            var values = new object[]
            {
                softwareInfo?.Product,
                softwareInfo?.Source,
                cameraModel?.Camera,
                cameraModel?.Model
            };

            yield return ServiceActivator.Create<EosInstallActionCreator>(types, values);
            yield return ServiceActivator.Create<PsInstallActionCreator>(types, values);
            yield return ServiceActivator.Create<ScriptActionCreator>(types, values);
        }

        private IEnumerable<IAction> GetActions(IActionCreator creator)
        {
            return creator.GetProducts(CardViewModel?.SelectedItem, CameraViewModel?.Info)
                .SelectMany(p => GetActions(creator, p));
        }

        protected virtual IEnumerable<IAction> GetActions(IActionCreator creator, SoftwareProductInfo product)
        {
            return GetSources(creator, product)
                .Select(s => CreateAction(creator, s));
        }

        protected abstract IEnumerable<ProductSource> GetSources(IActionCreator creator, SoftwareProductInfo product);

        protected static TAction CreateAction(IActionCreator creator, ProductSource s)
        {
            return creator.CreateAction<TAction>(s);
        }
    }
}
