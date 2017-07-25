using Chimp.Installers;
using Chimp.Model;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Chimp.Providers
{
    sealed class InstallerProvider : Provider<InstallerData, IInstaller>, IInstallerProvider
    {
        private InstallersData InstallersData { get; }

        public InstallerProvider(IServiceActivator serviceProvider, IOptions<InstallersData> options)
            : base(serviceProvider)
        {
            InstallersData = options.Value;
        }

        public IInstaller GetInstaller(string fileSystem)
        {
            if (!Data.TryGetValue(fileSystem ?? string.Empty, out InstallerData data))
                return null;
            return CreateProvider(fileSystem, data.Assembly, data.Type);
        }

        protected override IDictionary<string, InstallerData> Data => InstallersData.Installers;

        protected override string Namespace => typeof(Installer).Namespace;

        protected override string TypeSuffix => nameof(Installer);
    }
}
