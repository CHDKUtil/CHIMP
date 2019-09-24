using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Model.Card;
using System.IO;

namespace Chimp.Services
{
    sealed class InstallService : IInstallService
    {
        private ILogger Logger { get; }
        private MainViewModel MainViewModel { get; }
        private IPartitionService PartitionService { get; }
        private IFormatService FormatService { get; }
        private IBootService BootService { get; }
        private IDialogService DialogService { get; }

        private CardViewModel? CardViewModel => CardViewModel.Get(MainViewModel);
        private DownloadViewModel? DownloadViewModel => DownloadViewModel.Get(MainViewModel);
        private InstallViewModel? InstallViewModel => InstallViewModel.Get(MainViewModel);

        private CardInfo? Card => CardViewModel?.SelectedItem?.Info;
        private string? CategoryName => DownloadViewModel?.Software?.Category?.Name;

        private string[]? Paths => DownloadViewModel?.Paths;
        private string? CommonPath => Paths?.Length > 1
            ? Paths[0]
            : null;
        private string? SourcePath => Paths?[Paths.Length - 1];
        private string? DestPath => Card?.GetRootPath();

        public InstallService(MainViewModel mainViewModel, IPartitionService partitionService, IFormatService formatService, IBootService bootService, IDialogService dialogService, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<InstallService>();

            MainViewModel = mainViewModel;
            PartitionService = partitionService;
            FormatService = formatService;
            BootService = bootService;
            DialogService = dialogService;
        }

        public bool? TestSwitchedPartitions()
        {
            return CardViewModel?.SelectedItem?.Switched;
        }

        public void CreatePartitions()
        {
            SetTitle(nameof(Resources.Install_Partitioning_Text));
            var driveLetter = Card?.DriveLetter!;
            PartitionService.CreatePartitions(driveLetter);
            PartitionService.UpdateProperties(driveLetter);
        }

        public void SwitchPartitions()
        {
            SetTitle(nameof(Resources.Install_Switching_Text));
            var driveLetter = Card?.DriveLetter!;
            PartitionService.SwitchPartitions(driveLetter, 1);
            PartitionService.UpdateProperties(driveLetter);
        }

        public bool Format(string fileSystem, string label)
        {
            SetTitle(nameof(Resources.Install_Formatting_Text));
            return FormatService.Format(Card!, fileSystem, label);
        }

        public bool SetBootable(string fileSystem)
        {
            return BootService.SetBootable(Card!, fileSystem, CategoryName!, true);
        }

        public bool CopyPrimaryFiles()
        {
            SetTitle(nameof(Resources.Install_Copying_Text));

            if (SourcePath == null || DestPath == null)
                return false;

            CopyFiles(SourcePath, DestPath);

            var metadataPath = Path.Combine(SourcePath, Directories.Metadata);
            CopyDirectory(metadataPath, DestPath);

            return SetBootable("FAT");
        }

        public bool CopySecondaryFiles()
        {
            SetTitle(nameof(Resources.Install_Copying_Text));

            if (SourcePath == null || DestPath == null)
                return false;

            if (CommonPath != null)
                CopyAllDirectories(CommonPath, DestPath);
            return CopyAllFiles(SourcePath, DestPath);
        }

        public bool CopyAllFiles()
        {
            SetTitle(nameof(Resources.Install_Copying_Text));

            if (SourcePath == null || DestPath == null)
                return false;

            if (CommonPath != null)
                CopyAllDirectories(CommonPath, DestPath);
            return CopyAllFiles(SourcePath, DestPath);
        }

        public bool ShowFormatWarning()
        {
            SetTitle(nameof(Resources.Install_Format_Text));

            var message = Resources.MessageBox_Format_Message;
            var caption = CardViewModel?.SelectedItem?.DisplayName!;
            var result = DialogService.ShowOkCancelMessage(message, caption);
            if (!result)
                SetTitle(nameof(Resources.Install_Aborted_Text));

            return result;
        }

        private void CopyAllDirectories(string srcPath, string destPath)
        {
            foreach (var dir in Directory.EnumerateDirectories(srcPath))
            {
                CopyDirectory(dir, destPath);
            }
        }

        private bool CopyAllFiles(string srcPath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Logger.LogTrace("Creating {0}", destPath);
                Directory.CreateDirectory(destPath);
            }

            CopyFiles(srcPath, destPath);
            CopyAllDirectories(srcPath, destPath);

            return true;
        }

        private void CopyFiles(string srcPath, string destPath)
        {
            foreach (var file in Directory.EnumerateFiles(srcPath))
            {
                CopyFile(file, destPath);
            }
        }

        private void CopyDirectory(string dir, string destPath)
        {
            var dirName = Path.GetFileName(dir);
            var destDirPath = Path.Combine(destPath, dirName);
            SetFileName(destDirPath);
            CopyAllFiles(dir, destDirPath);
            UpdateEntryStart();
        }

        private void CopyFile(string file, string? destPath)
        {
            var fileName = Path.GetFileName(file);
            var destFilePath = Path.Combine(destPath, fileName);
            Logger.LogTrace("Copying {0}", destFilePath);
            SetFileName(destFilePath);
            File.Copy(file, destFilePath, true);
            UpdateEntryStart();
        }

        private void UpdateEntryStart()
        {
            InstallViewModel!.ProgressValue++;
        }

        private void SetTitle(string title)
        {
            Logger.LogInformation(title);
            InstallViewModel!.Title = title;
            InstallViewModel!.FileName = string.Empty;
        }

        private void SetFileName(string fileName)
        {
            if (DestPath == null)
                return;
            InstallViewModel!.FileName = fileName
                .Substring(DestPath.Length)
                .Replace(Path.DirectorySeparatorChar, '/')
                .ToUpperInvariant();
        }
    }
}
