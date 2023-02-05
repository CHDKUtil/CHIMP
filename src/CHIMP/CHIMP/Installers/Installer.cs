using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Camera;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Installers
{
    abstract class Installer : IInstaller
    {
        private const ulong MaxSdscCardSize = (ulong)2 * 1024 * 1024 * 1024;
        private const ulong MaxSdhcCardSize = (ulong)32 * 1024 * 1024 * 1024;

        private const string SDSC = "SDSC";
        private const string SDHC = "SDHC";

        private const ulong MaxFatCardSize = (ulong)4 * 1024 * 1024 * 1024;

        protected const string FAT = "FAT";
        protected const string FAT32 = "FAT32";
        protected const string exFAT = "exFAT";

        private static string DefaultLabel => Settings.Default.VolumeLabel;
        private static string DefaultSmallLabel => Settings.Default.VolumeSmallLabel;
        private static string DefaultLargeLabel => Settings.Default.VolumeLargeLabel;

        protected ILogger Logger { get; }
        private MainViewModel MainViewModel { get; }
        private IInstallService InstallService { get; }

        private string CardSubtype { get; }
        private string BootFileSystem { get; }

        private CardViewModel CardViewModel => CardViewModel.Get(MainViewModel);
        private CameraViewModel CameraViewModel => CameraViewModel.Get(MainViewModel);
        private DownloadViewModel DownloadViewModel => DownloadViewModel.Get(MainViewModel);
        private InstallViewModel InstallViewModel => InstallViewModel.Get(MainViewModel);
        private CardInfo Card => CardViewModel.SelectedItem.Info;
        private CameraInfo Camera => CameraViewModel.Info;
        private SoftwareProductInfo Product => DownloadViewModel.Software.Product;

        protected Installer(MainViewModel mainViewModel, IInstallService installService, ICameraProvider cameraProvider, ILogger logger)
        {
            Logger = logger;
            MainViewModel = mainViewModel;
            InstallService = installService;

            var cameraModels = cameraProvider.GetCameraModels(Camera);
            CardSubtype = cameraModels?.CardSubtype;
            BootFileSystem = DownloadViewModel.Software.Category.Name != "SCRIPT" 
                ? cameraModels?.BootFileSystem
                : FAT32;
            IsCameraMultiPartition = cameraModels?.IsMultiPartition == true;
            
        }

        public async Task<bool> InstallAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (!IsSupportedSize())
                {
                    SetTitle(nameof(Resources.Install_UnsupportedCardSize_Text));
                    return false;
                }
                return await Task.Run(() => Install(cancellationToken), cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogError(0, ex, "Canceled");
                return false;
            }
        }

        private bool IsSupportedSize()
        {
            if (TestSwitchedPartitions() == true)
            {
                return true;
            }
            switch (CardSubtype)
            {
                case SDSC:
                    return Card.Capacity <= MaxSdscCardSize;
                case SDHC:
                    return Card.Capacity <= MaxSdhcCardSize;
                default:
                    return true;
            }
        }

        protected abstract bool Install(CancellationToken cancellationToken);

        protected bool IsCameraMultiPartition { get; }

        protected bool IsCardFatFormattable =>
            Card.Capacity <= MaxFatCardSize;

        protected bool IsCameraExFatBootable =>
            exFAT.Equals(BootFileSystem, StringComparison.InvariantCulture);

        protected bool IsCameraFat32Bootable =>
            new[] { FAT32, exFAT }.Contains(BootFileSystem, StringComparer.InvariantCulture);

        protected bool CopySingle()
        {
            CopyAllFiles();

            if (!SetBootable(Card.FileSystem))
                return false;

            return true;
        }

        protected bool CopyDual()
        {
            if (!CopyPrimaryFiles())
                return false;

            SwitchPartitions();
            CopySecondaryFiles();
            SwitchPartitions();

            return true;
        }

        protected bool CopySwitchedDual()
        {
            if (!CopySecondaryFiles())
                return false;

            SwitchPartitions();
            CopyPrimaryFiles();
            SwitchPartitions();

            return true;
        }

        protected bool CopyFormat(string fileSystem)
        {
            if (!ShowFormatWarning())
                return false;

            if (!Format(fileSystem, Card.Label ?? DefaultLabel))
                return false;

            CopyAllFiles();

            if (!SetBootable(fileSystem))
                return false;

            return true;
        }

        protected bool CopyPartition()
        {
            if (!ShowFormatWarning())
                return false;

            CreatePartitions();

            if (!Format(FAT, Card.Label ?? DefaultSmallLabel))
                return false;

            CopyPrimaryFiles();

            SwitchPartitions();

            if (!Format(FAT32, DefaultLargeLabel))
                return false;

            CopySecondaryFiles();

            SwitchPartitions();

            return true;
        }

        protected bool? TestSwitchedPartitions()
        {
            return InstallService.TestSwitchedPartitions();
        }

        private void CreatePartitions()
        {
            InstallService.CreatePartitions();
        }

        private void SwitchPartitions()
        {
            InstallService.SwitchPartitions();
        }

        private bool Format(string fileSystem, string label)
        {
            return InstallService.Format(fileSystem, label);
        }

        private bool SetBootable(string fileSystem)
        {
            return InstallService.SetBootable(fileSystem);
        }

        private void CopyAllFiles()
        {
            InstallService.CopyAllFiles();
        }

        private bool CopyPrimaryFiles()
        {
            return InstallService.CopyPrimaryFiles();
        }

        private bool CopySecondaryFiles()
        {
            return InstallService.CopySecondaryFiles();
        }

        private bool ShowFormatWarning()
        {
            return InstallService.ShowFormatWarning();
        }

        private void SetTitle(string title)
        {
            Logger.LogInformation(title);
            InstallViewModel.Title = title;
            InstallViewModel.FileName = string.Empty;
        }
    }
}
