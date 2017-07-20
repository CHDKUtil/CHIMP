using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Camera;
using System.Threading;

namespace Chimp.Installers
{
    sealed class FatInstaller : Installer
    {
        public FatInstaller(MainViewModel mainViewModel, IInstallService installService, ICameraProvider cameraProvider, ILogger<FatInstaller> logger)
            : base(mainViewModel, installService, cameraProvider, logger)
        {
        }

        protected override bool Install(CancellationToken cancellationToken)
        {
            switch (TestSwitchedPartitions())
            {
                default:
                    return CopySingle();

                case false:
                    return CopyDual();

                case true:
                    return CopySwitchedDual();
            }
        }
    }
}
