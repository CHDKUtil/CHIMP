using Chimp.Model;
using Net.Chdk;
using System.IO;

namespace Chimp.Providers
{
    sealed class InstallerProvider : Provider<InstallerData, IInstaller>, IInstallerProvider
    {
        public InstallerProvider(IServiceActivator serviceProvider)
            : base(serviceProvider)
        {
        }

        public IInstaller GetInstaller(string fileSystem)
        {
            if (!Data.TryGetValue(fileSystem ?? string.Empty, out InstallerData data))
                return null;
            return CreateProvider(data.Assembly, data.Type);
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, "installers.json");
        }

        protected override string Namespace => "Chimp.Installers";

        protected override string TypeSuffix => "Installer";
    }
}
