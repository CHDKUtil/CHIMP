using Chimp.Controllers;
using Chimp.Model;
using Chimp.Providers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chimp.Containers
{
    sealed class ControllerContainer : Provider<StepData, IController>, IControllerContainer
    {
        private readonly Dictionary<string, IController> controllers = new Dictionary<string, IController>();

        private StepsData StepsData { get; }

        public ControllerContainer(IServiceActivator serviceActivator, IOptions<StepsData> options)
            : base(serviceActivator)
        {
            StepsData = options.Value;
        }

        public void Dispose()
        {
            foreach (var kvp in controllers)
                kvp.Value.Dispose();
        }

        public async Task<IController> GetControllerAsync(string name)
        {
            if (!controllers.TryGetValue(name, out IController controller))
            {
                //TODO lock
                controller = await CreateControllerAsync(name);
                controllers.Add(name, controller);
            }
            return controller;
        }

        private async Task<IController> CreateControllerAsync(string name)
        {
            var types = new[] { typeof(string) };
            var values = new[] { name };
            var controller = CreateProvider(name, Data[name].Assembly, name, types, values);
            await controller.InitializeAsync();
            return controller;
        }

        protected override IDictionary<string, StepData> Data =>
            StepsData.Steps.ToDictionary(
                s => s.Name,
                s => s);

        protected override string GetNamespace(string key)
        {
            return Data[key].Namespace
                ?? typeof(Controller<>).Namespace;
        }

        protected override string GetTypeSuffix() => "Controller";
    }
}
