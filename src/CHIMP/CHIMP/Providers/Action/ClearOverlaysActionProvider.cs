using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Providers.Substitute;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class ClearOverlaysActionProvider : ActionProvider
    {
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

            var software = SoftwareViewModel?.SelectedItem?.Info;
            if (software?.Product?.Name == "clear_overlays")
                yield break;

            var substitues = GetSubstitutes();
            if (substitues == null)
                yield break;

            var types = new[]
            {
                typeof(IDictionary<string, string>)
            };
            var values = new object[]
            {
                substitues
            };
            yield return ServiceActivator.Create<ClearOverlaysAction>(types, values);
        }

        private IDictionary<string, string> GetSubstitutes()
        {
            var camera = CameraViewModel?.Info;
            var cameraModel = CameraViewModel?.SelectedItem?.Model;
            if (camera == null || cameraModel == null)
                return null;

            return SubstituteProvider.GetSubstitutes(camera, cameraModel);
        }
    }
}
