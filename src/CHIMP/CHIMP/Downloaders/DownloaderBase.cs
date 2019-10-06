using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    abstract class DownloaderBase : IDownloader
    {
        protected ILogger Logger { get; }

        protected MainViewModel MainViewModel { get; }
        protected DownloadViewModel ViewModel => DownloadViewModel.Get(MainViewModel);

        private ISupportedProvider SupportedProvider { get; }

        protected DownloaderBase(MainViewModel mainViewModel, ISupportedProvider supportedProvider, ILogger logger)
        {
            MainViewModel = mainViewModel;
            SupportedProvider = supportedProvider;
            Logger = logger;
        }

        public abstract Task<SoftwareData> DownloadAsync(SoftwareCameraInfo camera, SoftwareInfo software, CancellationToken cancellationToken);

        protected void SetTitle(string title, LogLevel logLevel = LogLevel.Information)
        {
            Logger.Log(logLevel, default, title, null, null);
            ViewModel.Title = title;
            ViewModel.FileName = string.Empty;
        }

        protected void SetSupportedItems(SoftwareProductInfo product, SoftwareCameraInfo camera, MatchData data)
        {
            var error = SupportedProvider.GetError(data)
                ?? Resources.Download_UnsupportedModel_Text;
            SetTitle(error, LogLevel.Error);
            ViewModel.SupportedItems = SupportedProvider.GetItems(data, product, camera);
            ViewModel.SupportedTitle = SupportedProvider.GetTitle(data);
        }
    }
}
