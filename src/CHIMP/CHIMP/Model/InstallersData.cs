using System.Collections.Generic;

namespace Chimp.Model
{
    sealed class InstallerData
    {
        public string Assembly { get; set; }
        public string Type { get; set; }
    }

    sealed class InstallersData
    {
        public Dictionary<string, InstallerData> Installers { get; set; }
    }
}
