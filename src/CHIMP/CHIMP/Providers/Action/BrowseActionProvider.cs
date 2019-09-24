using Chimp.Actions;
using Chimp.ViewModels;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class BrowseActionProvider : ActionProvider
    {
        public BrowseActionProvider(MainViewModel mainViewModel, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
        }

        public override IEnumerable<IAction> GetActions()
        {
            if (SoftwareViewModel?.SelectedItem == null)
                yield return ServiceActivator.Create<BrowseAction>();
        }
    }
}
