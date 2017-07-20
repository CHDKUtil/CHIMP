using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Actions
{
    sealed class BrowseAction : ActionBase
    {
        private ILogger Logger { get; }
        private IDialogService DialogService { get; }
        private IExtractService ExtractService { get; }

        public BrowseAction(MainViewModel mainViewModel, IDialogService dialogService, IExtractService extractService, ILoggerFactory loggerFactory)
            : base(mainViewModel)
        {
            Logger = loggerFactory.CreateLogger<BrowseAction>();
            DialogService = dialogService;
            ExtractService = extractService;
        }

        public override async Task<SoftwareData> PerformAsync(CancellationToken cancellationToken)
        {
            var fileNames = DialogService.ShowOpenFileDialog(
                title: "Select Package",
                //title: Resources.Action_Browse_Dialog_Title,
                filter: "All files|*.*",
                multiselect: true);

            if (fileNames == null)
            {
                //DownloadViewModel.Title = Resources.Action_Browse_Aborted_Text;
                return null;
            }

            var tempPath = Path.Combine(Path.GetTempPath(), "CHIMP");
            Directory.CreateDirectory(tempPath);

            foreach (var filePath in fileNames)
            {
                await ExtractAsync(filePath, tempPath, cancellationToken);
            }

            //TODO

            return null;
        }

        private async Task<string> ExtractAsync(string path, string tempPath, CancellationToken cancellationToken)
        {
            var dirName = Path.GetFileNameWithoutExtension(path);
            var dirPath = Path.Combine(tempPath, dirName);

            if (Directory.Exists(dirPath))
            {
                Logger.LogTrace("Skipping {0}", dirPath);
                return dirPath;
            }

            var targetPath = path;
            var fileName = Path.GetFileName(path);
            var filePath = Path.Combine(tempPath, fileName);

            return await ExtractAsync(targetPath: targetPath, filePath: filePath, dirPath: dirPath, tempPath: tempPath, cancellationToken: cancellationToken);
        }

        private async Task<string> ExtractAsync(string targetPath, string filePath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            //SetTitle(nameof(Resources.Download_Extracting_Text));
            DownloadViewModel.ProgressMaximum = 0;

            try
            {
                return await Task.Run(() => ExtractService.Extract(
                    path: targetPath,
                    filePath: filePath,
                    dirPath: dirPath,
                    tempPath: tempPath,
                    cancellationToken: cancellationToken),
                cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogWarning(0, ex, "Canceled");
                return null;
            }
        }

        public override string DisplayName => "Install from Disk";
    }
}
