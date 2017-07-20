using Chimp.Providers;
using Chimp.Providers.Action;
using Net.Chdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chimp.Containers
{
    sealed class ActionContainer : Provider<IActionProvider>, IActionProvider
    {
        public ActionContainer(IServiceActivator serviceActivator)
            : base(serviceActivator)
        {
            _actions = new Lazy<IEnumerable<IAction>>(CreateActions);
        }

        public IEnumerable<IAction> GetActions()
        {
            return Actions;
        }

        private IActionProvider CreateProvider(KeyValuePair<string, string> kvp)
        {
            return CreateProvider(kvp.Value, kvp.Key);
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, "actions.json");
        }

        protected override string Namespace => typeof(ActionProvider).Namespace;

        protected override string TypeSuffix => nameof(ActionProvider);

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
