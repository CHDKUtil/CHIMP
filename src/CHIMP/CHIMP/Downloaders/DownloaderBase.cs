using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    abstract class DownloaderBase : IDownloader
    {
        protected ILogger Logger { get; }

        private MainViewModel MainViewModel { get; }
        private SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);
        
        protected DownloadViewModel ViewModel => DownloadViewModel.Get(MainViewModel);

        private IMetadataService MetadataService { get; }
        private ISupportedProvider SupportedProvider { get; }

        protected DownloaderBase(MainViewModel mainViewModel, IMetadataService metadataService, ISupportedProvider supportedProvider, ILogger logger)
        {
            MainViewModel = mainViewModel;
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

        protected abstract Task<SoftwareData> GetSoftwareAsync(SoftwareInfo softwareInfo, CancellationToken cancellationToken);

        protected abstract Task<string[]> DownloadExtractAsync(SoftwareData software, CancellationToken cancellationToken);

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

        protected void SetSupportedItems(MatchData data, SoftwareInfo software)
        {
            var error = SupportedProvider.GetError(data)
                ?? Resources.Download_UnsupportedModel_Text;
            SetTitle(error, LogLevel.Error);
            ViewModel.SupportedItems = SupportedProvider.GetItems(data, software);
            ViewModel.SupportedTitle = SupportedProvider.GetTitle(data);
        }
    }
}
