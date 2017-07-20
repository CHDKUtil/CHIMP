using Chimp.Providers;
using Net.Chdk;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Chimp.Containers
{
    sealed class ControllerContainer : Provider<IController>, IControllerContainer
    {
        private readonly Dictionary<string, IController> controllers = new Dictionary<string, IController>();

        public ControllerContainer(IServiceActivator serviceProvider)
            : base(serviceProvider)
        {
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
            var controller = CreateProvider(Data[name], name);
            await controller.InitializeAsync();
            return controller;
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, "steps.json");
        }

        protected override string Namespace => "Chimp.Controllers";

        protected override string TypeSuffix => "Controller";
    }
}
