using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    sealed class Downloader : DownloaderBase
    {
        private SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);

        private IBuildProvider BuildProvider { get; }
        private IMatchProvider MatchProvider { get; }
        private ISoftwareProvider SoftwareProvider { get; }
        private IDownloadProvider DownloadProvider { get; }

        private IDownloadService DownloadService { get; }
        private IExtractService ExtractService { get; }
        private IMetadataService MetadataService { get; }

        public Downloader(MainViewModel mainViewModel,
            IBuildProvider buildProvider, IMatchProvider matchProvider, ISoftwareProvider softwareProvider, IDownloadProvider downloadProvider,
            IDownloadService downloadService, IExtractService extractService, IMetadataService metadataService, ILogger<Downloader> logger)
                : base(mainViewModel, logger)
        {
            BuildProvider = buildProvider;
            MatchProvider = matchProvider;
            SoftwareProvider = softwareProvider;
            DownloadProvider = downloadProvider;

            DownloadService = downloadService;
            ExtractService = extractService;
            MetadataService = metadataService;
        }

        public override async Task<SoftwareData> DownloadAsync(SoftwareInfo softwareInfo, CancellationToken cancellationToken)
        {
            var software = await GetSoftwareAsync(softwareInfo, cancellationToken);
            if (software == null)
                return null;

            if (TrySkipUpToDate(software))
                return null;

            var paths = await DownloadExtractAsync(software, cancellationToken);
            if (paths == null || paths.Any(p => p == null))
                return null;

            software.Info = await UpdateAsync(software.Info, paths.Last(), cancellationToken);
            software.Paths = paths;

            return software;
        }

        private async Task<SoftwareData> GetSoftwareAsync(SoftwareInfo softwareInfo, CancellationToken cancellationToken)
        {
            SetTitle(nameof(Resources.Download_FetchingData_Text));

            var camera = softwareInfo.Camera;
            var buildName = BuildProvider.GetBuildName(softwareInfo);
            var result = await MatchProvider.GetMatchesAsync(camera, buildName, cancellationToken);
            var matches = result.Matches;
            if (matches == null)
            {
                SetTitle(result.Error, LogLevel.Error);
                ViewModel.SupportedItems = GetSupportedItems(result).ToArray();
                ViewModel.SupportedTitle = GetSupportedTitle(result);
                return null;
            }

            var match = matches.Last();
            var info = SoftwareProvider.GetSoftware(match);
            var downloads = DownloadProvider.GetDownloads(matches, info).ToArray();

            return new SoftwareData
            {
                Info = info,
                Downloads = downloads,
            };
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

            var filePath = await DownloadAsync(download, path: path, targetPath: targetPath, tempPath: tempPath, cancellationToken: cancellationToken);
            if (filePath == null)
                return null;

            return await ExtractAsync(targetPath: targetPath, filePath: filePath, dirPath: dirPath, tempPath: tempPath, cancellationToken: cancellationToken);
        }

        private async Task<string> DownloadAsync(DownloadData download, string path, string targetPath, string tempPath, CancellationToken cancellationToken)
        {
            var fileName = Path.GetFileName(targetPath);
            var filePath = Path.Combine(tempPath, fileName);
            if (File.Exists(filePath))
            {
                Logger.LogTrace("Skipping {0}", filePath);
                return filePath;
            }

            SetTitle(nameof(Resources.Download_Downloading_Text));
            ViewModel.FileName = Path.GetFileName(path);
            TryParseSize(download.Size, out int size);
            ViewModel.ProgressMaximum = size;

            try
            {
                return await DownloadService.DownloadAsync(
                    baseUri: download.BaseUri,
                    path: path,
                    filePath: filePath,
                    cancellationToken: cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogError(0, ex, "Canceled");
                //cts = null;
                return null;
            }
        }

        private async Task<string> ExtractAsync(string targetPath, string filePath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            SetTitle(nameof(Resources.Download_Extracting_Text));
            ViewModel.ProgressMaximum = 0;

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
                //cts = null;
                return null;
            }
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

        private bool TrySkipUpToDate(SoftwareData software)
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

        private static bool TryParseSize(string sizeStr, out int size)
        {
            size = 0;
            if (string.IsNullOrEmpty(sizeStr))
                return false;

            if (sizeStr.EndsWith("M"))
            {
                sizeStr = sizeStr.Substring(0, sizeStr.Length - 1);
                if (!double.TryParse(sizeStr, out double sizeDouble))
                    return false;
                size = (int)(sizeDouble * 1048576);
                return true;
            }

            if (sizeStr.EndsWith("K"))
            {
                sizeStr = sizeStr.Substring(0, sizeStr.Length - 1);
                if (!double.TryParse(sizeStr, out double sizeDouble))
                    return false;
                size = (int)(sizeDouble * 1024);
                return true;
            }

            return int.TryParse(sizeStr, out size);
        }

        private IEnumerable<string> GetSupportedItems(MatchData result)
        {
            if (result.Builds != null)
                return GetSupportedBuilds(result.Builds);
            if (result.Revisions != null)
                return GetSupportedRevisions(result.Revisions);
            if (result.Platforms != null)
                return GetSupportedModels(result.Platforms);
            return null;
        }

        private string GetSupportedTitle(MatchData result)
        {
            if (result.Builds != null)
                return GetSupportedBuildsTitle(result.Builds);
            if (result.Revisions != null)
                return GetSupportedRevisionsTitle(result.Revisions);
            if (result.Platforms != null)
                return GetSupportedModelsTitle(result.Platforms);
            return null;
        }

        private IEnumerable<string> GetSupportedBuilds(IEnumerable<string> _)
        {
            return null;
        }

        private string GetSupportedBuildsTitle(IEnumerable<string> _)
        {
            return null;
        }
    }
}
