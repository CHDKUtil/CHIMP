using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    abstract class DownloaderBase : IDownloader
    {
        protected ILogger Logger { get; }

        protected MainViewModel MainViewModel { get; }
        protected DownloadViewModel ViewModel => DownloadViewModel.Get(MainViewModel);

        protected DownloaderBase(MainViewModel mainViewModel, ILogger logger)
        {
            MainViewModel = mainViewModel;
            Logger = logger;
        }

        public abstract Task<SoftwareData> DownloadAsync(SoftwareInfo software, CancellationToken cancellationToken);

        protected void SetTitle(string title, LogLevel logLevel = LogLevel.Information)
        {
            Logger.Log(logLevel, default, title, null, null);
            ViewModel.Title = title;
            ViewModel.FileName = string.Empty;
        }

        protected static IEnumerable<string> GetSupportedModels(IEnumerable<string> _)
        {
            return Enumerable.Empty<string>();
        }

        protected static string GetSupportedModelsTitle(IEnumerable<string> platforms)
        {
            return platforms.Count() > 1
                ? Resources.Download_SupportedModels_Content
                : Resources.Download_SupportedModel_Content;
        }

        protected static IEnumerable<string> GetSupportedRevisions(IEnumerable<string> revisions)
        {
            return revisions.Select(GetRevision);
        }

        protected static string GetSupportedRevisionsTitle(IEnumerable<string> revisions)
        {
            return revisions.Count() > 1
                ? Resources.Download_SupportedFirmwares_Content
                : Resources.Download_SupportedFirmware_Content;
        }

        private static string GetRevision(string value)
        {
            switch (value.Length)
            {
                case 3:
                    return $"{value[0]}.{value[1]}.{value[2]}";
                case 4:
                    return string.Format(Resources.Camera_FirmwareVersion_Format, value[0], value[1], value[2], value[3] - 'a' + 1, 0, 0);
                default:
                    return null;
            }
        }
    }
}
