using Chimp.Actions;
using Chimp.ViewModels;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    sealed class EjectActionProvider : ActionProvider
    {
        public EjectActionProvider(MainViewModel mainViewModel, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
        }

        public override IEnumerable<IAction> GetActions()
        {
            if (EjectViewModel.IsEject)
                yield return ServiceActivator.Create<EjectAction>();
        }
    }
}
