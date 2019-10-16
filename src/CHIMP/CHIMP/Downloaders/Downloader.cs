using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Software;
using Net.Chdk.Providers.Supported;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    sealed class Downloader : Downloader<MatchData, DownloadData, ExtractData, Match[]>
    {
        private IDownloadService DownloadService { get; }
        private IExtractService ExtractService { get; }

        public Downloader(MainViewModel mainViewModel, ISupportedProvider supportedProvider,
            BuildProvider buildProvider, MatchProvider matchProvider, SoftwareProvider softwareProvider, DownloadProvider downloadProvider,
            IDownloadService downloadService, IExtractService extractService, IMetadataService metadataService, ILogger<Downloader> logger)
                : base(mainViewModel, buildProvider, matchProvider, softwareProvider, downloadProvider, metadataService, supportedProvider, logger)
        {
            DownloadService = downloadService;
            ExtractService = extractService;
        }

        protected override async Task<ExtractData> DownloadAsync(DownloadData download, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            var path = download.Path;
            var fileName = Path.GetFileName(targetPath);
            var filePath = Path.Combine(tempPath, fileName);
            if (File.Exists(filePath))
            {
                Logger.LogTrace("Skipping {0}", filePath);
                return new ExtractData(filePath);
            }

            SetTitle(nameof(Resources.Download_Downloading_Text));
            ViewModel.FileName = Path.GetFileName(path);
            TryParseSize(download.Size, out int size);
            ViewModel.ProgressMaximum = size;

            try
            {
                filePath = await DownloadService.DownloadAsync(
                    baseUri: download.BaseUri,
                    path: path,
                    filePath: filePath,
                    cancellationToken: cancellationToken);
                return new ExtractData(filePath);
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogError(0, ex, "Canceled");
                //cts = null;
                return null;
            }
        }

        protected override async Task<string> ExtractAsync(ExtractData extract, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            SetTitle(nameof(Resources.Download_Extracting_Text));
            ViewModel.ProgressMaximum = 0;

            try
            {
                return await Task.Run(() => ExtractService.Extract(
                    path: targetPath,
                    filePath: extract.FilePath,
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
    }
}
