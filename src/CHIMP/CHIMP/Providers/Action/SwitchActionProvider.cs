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
            var types = CardViewModel?.SelectedItem?.PartitionTypes;
            if (types != null && types[0] != PartitionType.None)
            {
                for (int i = 1; i < types.Length; i++)
                    if (types[i] != PartitionType.None)
                        yield return CreateAction(i);
            }
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
