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
            var substitues = GetSubstitutes();
            if (substitues == null)
                yield break;
            var types = new[] { typeof(IDictionary<string, string>) };
            var values = new object[] { substitues };
            yield return ServiceActivator.Create<ClearOverlaysAction>(types, values);
        }

        private IDictionary<string, string> GetSubstitutes()
        {
            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || card?.Bootable != null)
                return null;

            var camera = CameraViewModel?.Info;
            var cameraModel = CameraViewModel?.SelectedItem?.Model;
            if (camera == null || cameraModel == null)
                return null;

            return SubstituteProvider.GetSubstitutes(camera, cameraModel);
        }
    }
}
