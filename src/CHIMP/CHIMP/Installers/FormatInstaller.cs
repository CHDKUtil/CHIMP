using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.CameraModel;
using System.Threading;

namespace Chimp.Installers
{
    sealed class FormatInstaller : Installer
    {
        public FormatInstaller(MainViewModel mainViewModel, IInstallService installService, ICameraModelProvider cameraProvider, ILogger<FormatInstaller> logger)
            : base(mainViewModel, installService, cameraProvider, logger)
        {
        }

        protected override bool Install(CancellationToken cancellationToken)
        {
            if (IsCameraExFatBootable)
                return CopyFormat(exFAT);

            if (IsCameraFat32Bootable)
                return CopyFormat(FAT32);

            if (IsCardFatFormattable)
                return CopyFormat(FAT);

            return CopyPartition();
        }
    }
}
