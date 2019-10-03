using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.CameraModel;
using System.Threading;

namespace Chimp.Installers
{
    sealed class Fat32Installer : Installer
    {
        public Fat32Installer(MainViewModel mainViewModel, IInstallService installService, ICameraModelProvider cameraProvider, ILogger<Fat32Installer> logger)
            : base(mainViewModel, installService, cameraProvider, logger)
        {
        }

        protected override bool Install(CancellationToken cancellationToken)
        {
            if (TestSwitchedPartitions() == true && IsCameraMultiPartition)
                return CopySwitchedDual();

            if (IsCameraFat32Bootable)
                return CopySingle();

            if (IsCardFatFormattable)
                return CopyFormat(FAT);

            return CopyPartition();
        }
    }
}
