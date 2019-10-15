using Chimp.Model;
using Chimp.Providers;
using Chimp.Providers.Action;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Containers
{
    sealed class ActionContainer : Provider<ActionData, IActionProvider>, IActionProvider
    {
        private ActionsData ActionsData { get; }

        public ActionContainer(IServiceActivator serviceActivator, IOptions<ActionsData> options)
            : base(serviceActivator)
        {
            ActionsData = options.Value;
            _actions = new Lazy<IEnumerable<IAction>>(CreateActions);
        }

        public IEnumerable<IAction> GetActions()
        {
            return Actions;
        }

        private IActionProvider CreateProvider(KeyValuePair<string, ActionData> kvp)
        {
            return CreateProvider(kvp.Key, kvp.Value.Assembly, kvp.Key);
        }

        protected override IDictionary<string, ActionData> Data =>
            ActionsData.Actions.ToDictionary(
                a => a.Name,
                a => a);

        protected override string GetNamespace(string key)
        {
            return Data[key].Namespace
                ?? typeof(ActionProvider).Namespace;
        }

        protected override string GetTypeSuffix() => nameof(ActionProvider);

        #region Actions

        private readonly Lazy<IEnumerable<IAction>> _actions;

        private IEnumerable<IAction> Actions => _actions.Value;

        private IEnumerable<IAction> CreateActions()
        {
            return Data
                .Select(CreateProvider)
                .SelectMany(p => p.GetActions());
        }

        #endregion
    }
}
