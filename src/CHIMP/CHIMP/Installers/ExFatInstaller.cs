using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Camera;
using System.Threading;

namespace Chimp.Installers
{
    sealed class ExFatInstaller : Installer
    {
        public ExFatInstaller(MainViewModel mainViewModel, IInstallService installService, ICameraProvider cameraProvider, ILogger<ExFatInstaller> logger)
            : base(mainViewModel, installService, cameraProvider, logger)
        {
        }

        protected override bool Install(CancellationToken cancellationToken)
        {
            if (IsCameraExFatBootable)
                return CopySingle();

            if (IsCameraFat32Bootable)
                return CopyFormat(FAT32);

            if (IsCardFatFormattable)
                return CopyFormat(FAT);

            return CopyPartition();
        }
    }
}
