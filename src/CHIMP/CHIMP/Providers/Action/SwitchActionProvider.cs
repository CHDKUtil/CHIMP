using System.Collections.Generic;
using Chimp.ViewModels;
using Chimp.Actions;
using Chimp.Model;

namespace Chimp.Providers.Action
{
    sealed class SwitchActionProvider : ActionProvider
    {
        public SwitchActionProvider(MainViewModel mainViewModel, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
        }

        public override IEnumerable<IAction> GetActions()
        {
            PartitionType[] partTypes = CardViewModel.SelectedItem.PartitionTypes;
            if (partTypes == null || partTypes[0] == PartitionType.None)
                yield break;
            for (int i = 1; i < partTypes.Length; i++)
                if (partTypes[i] != PartitionType.None)
                    yield return CreateAction(i);
        }

        private IAction CreateAction(int part)
        {
            var types = new[]
            {
                typeof(int),
            };
            var args = new object[]
            {
                part,
            };
            return ServiceActivator.Create<SwitchAction>(types, args);
        }
    }
}
