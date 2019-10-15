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
    abstract class Downloader<TMatchData, TDownloadData, TExtractData, TPayload> : IDownloader
        where TMatchData : MatchData<TPayload>
        where TDownloadData : IDownloadData
        where TExtractData : ExtractData
        where TPayload : class
    {
        protected ILogger Logger { get; }

        private MainViewModel MainViewModel { get; }
        private IBuildProvider BuildProvider { get; }
        private IMatchProvider<TMatchData> MatchProvider { get; }
        private ISoftwareProvider<TMatchData> SoftwareProvider { get; }
        private IDownloadProvider<TMatchData, TDownloadData> DownloadProvider { get; }

        private SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);
        
        protected DownloadViewModel ViewModel => DownloadViewModel.Get(MainViewModel);

        private IMetadataService MetadataService { get; }
        private ISupportedProvider SupportedProvider { get; }

        protected Downloader(MainViewModel mainViewModel, IBuildProvider buildProvider, IMatchProvider<TMatchData> matchProvider, ISoftwareProvider<TMatchData> softwareProvider, IDownloadProvider<TMatchData, TDownloadData> downloadProvider,
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

            if (TrySkipUpToDate(software.Info))
                return null;

            var paths = await DownloadExtractAsync(software.Downloads, cancellationToken);
            if (paths == null)
                return null;

            var info = await UpdateAsync(software.Info, paths.Last(), cancellationToken);

            return new SoftwareData
            {
                Info = info,
                Paths = paths
            };
        }

        private async Task<DownloadsData<TDownloadData>> GetSoftwareAsync(SoftwareInfo softwareInfo, CancellationToken cancellationToken)
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
            var downloads = DownloadProvider.GetDownloads(result, info)?.ToArray();

            return new DownloadsData<TDownloadData>
            {
                Info = info,
                Downloads = downloads
            };
        }

        private async Task<string[]> DownloadExtractAsync(TDownloadData[] downloads, CancellationToken cancellationToken)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "CHIMP");
            Directory.CreateDirectory(tempPath);

            var paths = new string[downloads.Length];
            for (var i = 0; i < downloads.Length; i++)
            {
                var download = downloads[i];
                var path = await DownloadExtractAsync(download, tempPath, cancellationToken);
                if (path == null)
                    return null;
                paths[i] = path;
            }

            return paths;
        }

        private async Task<string> DownloadExtractAsync(TDownloadData download, string tempPath, CancellationToken cancellationToken)
        {
            var targetPath = download.TargetPath ?? download.Path;

            var destPath = await DownloadExtractAsync(download, targetPath: targetPath, tempPath: tempPath, cancellationToken: cancellationToken);
            if (destPath == null)
                return null;

            return download.RootDir != null
                ? Path.Combine(destPath, download.RootDir)
                : destPath;
        }

        private async Task<string> DownloadExtractAsync(TDownloadData download, string targetPath, string tempPath, CancellationToken cancellationToken)
        {
            var dirName = Path.GetFileNameWithoutExtension(targetPath);
            var dirPath = Path.Combine(tempPath, dirName);

            if (Directory.Exists(dirPath))
            {
                Logger.LogTrace("Skipping {0}", dirPath);
                return dirPath;
            }

            var extract = await DownloadAsync(download, targetPath: targetPath, dirPath: dirPath, tempPath: tempPath, cancellationToken: cancellationToken);
            if (extract == null)
                return null;

            return await ExtractAsync(extract, targetPath: targetPath, dirPath: dirPath, tempPath: tempPath, cancellationToken: cancellationToken);
        }

        protected abstract Task<TExtractData> DownloadAsync(TDownloadData download, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken);

        protected abstract Task<string> ExtractAsync(TExtractData extract, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken);

        protected bool TrySkipUpToDate(SoftwareInfo software)
        {
            var currentInfo = SoftwareViewModel.SelectedItem?.Info;
            var version = currentInfo?.Product?.Version;
            var language = currentInfo?.Product?.Language;
            if (software.Product.Version.Equals(version) && software.Product.Language.Equals(language))
            {
                //For status
                currentInfo.Build ??= software.Build;

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
