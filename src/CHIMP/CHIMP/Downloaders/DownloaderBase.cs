using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using Net.Chdk.Providers.Supported;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    abstract class DownloaderBase : IDownloader
    {
        protected ILogger Logger { get; }

        private MainViewModel MainViewModel { get; }
        private IBuildProvider BuildProvider { get; }
        private IMatchProvider MatchProvider { get; }
        private ISoftwareProvider SoftwareProvider { get; }
        private IDownloadProvider DownloadProvider { get; }

        private SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);
        
        protected DownloadViewModel ViewModel => DownloadViewModel.Get(MainViewModel);

        private IMetadataService MetadataService { get; }
        private ISupportedProvider SupportedProvider { get; }

        protected DownloaderBase(MainViewModel mainViewModel, IBuildProvider buildProvider, IMatchProvider matchProvider, ISoftwareProvider softwareProvider, IDownloadProvider downloadProvider,
            IMetadataService metadataService, ISupportedProvider supportedProvider, ILogger logger)
        {
            MainViewModel = mainViewModel;
            BuildProvider = buildProvider;
            MatchProvider = matchProvider;
            SoftwareProvider = softwareProvider;
            DownloadProvider = downloadProvider;
            MetadataService = metadataService;
            SupportedProvider = supportedProvider;
            Logger = logger;
        }

        public virtual async Task<SoftwareData> DownloadAsync(SoftwareInfo softwareInfo, CancellationToken cancellationToken)
        {
            var software = await GetSoftwareAsync(softwareInfo, cancellationToken);
            if (software == null)
                return null;

            if (TrySkipUpToDate(software))
                return null;

            var paths = await DownloadExtractAsync(software, cancellationToken);
            if (paths == null)
                return null;

            software.Info = await UpdateAsync(software.Info, paths.Last(), cancellationToken);
            software.Paths = paths;

            return software;
        }

        private async Task<SoftwareData> GetSoftwareAsync(SoftwareInfo softwareInfo, CancellationToken cancellationToken)
        {
            SetTitle(nameof(Resources.Download_FetchingData_Text));

            var buildName = BuildProvider.GetBuildName(softwareInfo);
            var result = await MatchProvider.GetMatchesAsync(softwareInfo, buildName, cancellationToken);
            if (!result.Success)
            {
                SetSupportedItems(result, softwareInfo);
                return null;
            }

            var info = SoftwareProvider.GetSoftware(result, softwareInfo);

            var software = new SoftwareData
            {
                Info = info,
                Match = result
            };
            
            software.Downloads = DownloadProvider.GetDownloads(software)?.ToArray();

            return software;
        }

        private async Task<string[]> DownloadExtractAsync(SoftwareData software, CancellationToken cancellationToken)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "CHIMP");
            Directory.CreateDirectory(tempPath);

            var paths = new string[software.Downloads.Length];
            for (var i = 0; i < software.Downloads.Length; i++)
            {
                var download = software.Downloads[i];
                var path = await DownloadExtractAsync(download, tempPath, cancellationToken);
                if (path == null)
                    return null;
                paths[i] = path;
            }

            return paths;
        }

        private async Task<string> DownloadExtractAsync(DownloadData download, string tempPath, CancellationToken cancellationToken)
        {
            var path = download.Path;
            var targetPath = download.TargetPath ?? path;

            var destPath = await DownloadExtractAsync(download, path: path, targetPath: targetPath, tempPath: tempPath, cancellationToken: cancellationToken);
            if (destPath == null)
                return null;

            return download.RootDir != null
                ? Path.Combine(destPath, download.RootDir)
                : destPath;
        }

        private async Task<string> DownloadExtractAsync(DownloadData download, string path, string targetPath, string tempPath, CancellationToken cancellationToken)
        {
            var dirName = Path.GetFileNameWithoutExtension(targetPath);
            var dirPath = Path.Combine(tempPath, dirName);

            if (Directory.Exists(dirPath))
            {
                Logger.LogTrace("Skipping {0}", dirPath);
                return dirPath;
            }

            var extract = await DownloadAsync(download, path: path, targetPath: targetPath, dirPath: dirPath, tempPath: tempPath, cancellationToken: cancellationToken);
            if (extract == null)
                return null;

            return await ExtractAsync(extract, targetPath: targetPath, dirPath: dirPath, tempPath: tempPath, cancellationToken: cancellationToken);
        }

        protected abstract Task<ExtractData> DownloadAsync(DownloadData download, string path, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken);

        protected abstract Task<string> ExtractAsync(ExtractData extract, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken);

        protected bool TrySkipUpToDate(SoftwareData software)
        {
            var currentInfo = SoftwareViewModel.SelectedItem?.Info;
            var version = currentInfo?.Product?.Version;
            var language = currentInfo?.Product?.Language;
            if (software.Info.Product.Version.Equals(version) && software.Info.Product.Language.Equals(language))
            {
                //For status
                currentInfo.Build = currentInfo.Build ?? software.Info.Build;

                SetTitle(nameof(Resources.Download_UpToDate_Text));
                ViewModel.Software = currentInfo;
                ViewModel.IsUpToDate = true;

                return true;
            }

            return false;
        }

        private async Task<SoftwareInfo> UpdateAsync(SoftwareInfo softwareInfo, string destPath, CancellationToken cancellationToken)
        {
            SetTitle(nameof(Resources.Download_Updating_Text));
            ViewModel.ProgressMaximum = 0;

            try
            {
                return await Task.Run(() => MetadataService.Update(softwareInfo, destPath, null, cancellationToken), cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogWarning(0, ex, "Canceled");
                //cts = null;
                return null;
            }
        }

        protected void SetTitle(string title, LogLevel logLevel = LogLevel.Information)
        {
            Logger.Log(logLevel, default, title, null, null);
            ViewModel.Title = title;
            ViewModel.FileName = string.Empty;
        }

        protected void SetSupportedItems(IMatchData data, SoftwareInfo softwareInfo)
        {
            var software = SoftwareProvider.GetSoftware(null, softwareInfo);
            var error = SupportedProvider.GetError(data)
                ?? Resources.Download_UnsupportedModel_Text;
            SetTitle(error, LogLevel.Error);
            ViewModel.SupportedItems = SupportedProvider.GetItems(data, software);
            ViewModel.SupportedTitle = SupportedProvider.GetTitle(data);
        }
    }
}
