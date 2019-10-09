using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Providers.Substitute;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class ClearOverlaysActionProvider : ActionProvider
    {
        private const string ProductName = "clear_overlays";

        private ISubstituteProvider SubstituteProvider { get; }

        public ClearOverlaysActionProvider(MainViewModel mainViewModel, ISubstituteProvider substituteProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            SubstituteProvider = substituteProvider;
        }

        public override IEnumerable<IAction> GetActions()
        {
            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || (card?.Bootable != null && card?.Bootable != "SCRIPT"))
                yield break;

            var softwareInfo = SoftwareViewModel?.SelectedItem?.Info;
            if (softwareInfo?.Product?.Name == ProductName)
                yield break;

            var substitues = GetSubstitutes();
            if (substitues == null)
                yield break;

            var types = new[]
            {
                typeof(IDictionary<string, object>)
            };
            var values = new object[]
            {
                substitues
            };
            yield return ServiceActivator.Create<ClearOverlaysAction>(types, values);
        }

        private IDictionary<string, object> GetSubstitutes()
        {
            var camera = CameraViewModel?.Info;
            var cameraModel = CameraViewModel?.SelectedItem?.Model;
            if (camera == null || cameraModel == null)
                return null;

            return SubstituteProvider.GetSubstitutes(camera, cameraModel);
        }
    }
}
