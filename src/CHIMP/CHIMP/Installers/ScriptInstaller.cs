using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Camera;
using System.Threading;

namespace Chimp.Installers
{
    sealed class ScriptInstaller : Installer
    {
        public ScriptInstaller(MainViewModel mainViewModel, IInstallService installService, ICameraProvider cameraProvider, ILogger<ScriptInstaller> logger)
            : base(mainViewModel, installService, cameraProvider, logger)
        {
        }

        protected override bool Install(CancellationToken cancellationToken)
        {
            return CopySingle();
        }
    }
}
