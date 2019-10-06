using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Camera;
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

        private ICameraProvider CameraProvider { get; }

        protected DownloaderBase(MainViewModel mainViewModel, ICameraProvider cameraProvider, ILogger logger)
        {
            MainViewModel = mainViewModel;
            CameraProvider = cameraProvider;
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
            var error = GetError(data);
            SetTitle(error, LogLevel.Error);
            ViewModel.SupportedItems = GetSupportedItems(product, camera, data);
            ViewModel.SupportedTitle = GetSupportedTitle(data);
        }

        private static string GetError(MatchData data)
        {
            if (data.Error != null)
                return data.Error;
            if (data.Builds != null)
                return Resources.Download_InvalidFormat_Text;
            if (data.Revisions != null)
                return Resources.Download_UnsupportedFirmware_Text;
            return Resources.Download_UnsupportedModel_Text;
        }

        private string[] GetSupportedItems(SoftwareProductInfo product, SoftwareCameraInfo camera, MatchData data)
        {
            if (data.Builds != null)
                return GetSupportedBuilds(data);
            if (data.Revisions != null)
                return GetSupportedRevisions(data);
            if (data.Platforms != null)
                return GetSupportedModels(product, camera, data);
            return null;
        }

        private static string GetSupportedTitle(MatchData data)
        {
            if (data.Builds != null)
                return GetSupportedBuildsTitle(data);
            if (data.Revisions != null)
                return GetSupportedRevisionsTitle(data);
            if (data.Platforms != null)
                return GetSupportedModelsTitle(data);
            return null;
        }

        private string[] GetSupportedModels(SoftwareProductInfo product, SoftwareCameraInfo camera, MatchData data)
        {
            return data.Platforms
                .SelectMany(p => GetModels(p, product, camera))
                .ToArray();
        }

        private static string GetSupportedModelsTitle(MatchData data)
        {
            return data.Platforms.Count() > 1
                ? Resources.Download_SupportedModels_Content
                : Resources.Download_SupportedModel_Content;
        }

        private static string[] GetSupportedRevisions(MatchData data)
        {
            return data.Revisions
                .Select(GetRevision)
                .ToArray();
        }

        private static string GetSupportedRevisionsTitle(MatchData data)
        {
            return data.Revisions.Count() > 1
                ? Resources.Download_SupportedFirmwares_Content
                : Resources.Download_SupportedFirmware_Content;
        }

        private static string[] GetSupportedBuilds(MatchData _)
        {
            return null;
        }

        private static string GetSupportedBuildsTitle(MatchData _)
        {
            return null;
        }

        private IEnumerable<string> GetModels(string platform, SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo)
        {
            var camera = GetCamera(platform, cameraInfo);
            var data = CameraProvider.GetCameraModels(productInfo, camera);
            if (data?.Models != null)
                foreach (var model in data.Models)
                    yield return GetModel(model);
        }

        private static string GetModel(CameraModelInfo model)
        {
            var models = Enumerable.Range(0, model.Names.Length)
                .Select(i => GetModel(model.Names, i));
            return string.Join(" / ", models);
        }

        private static string GetModel(string[] models, int index)
        {
            var model = models[index]
                .TrimStart("Canon ")
                .TrimEnd(" IS")
                .TrimEnd(" HS")
                .TrimEnd(" Ti");

            if (index > 0)
                model = model
                    .TrimStart("EOS ")
                    .TrimStart("PowerShot ");
            return model;
        }

        private static SoftwareCameraInfo GetCamera(string platform, SoftwareCameraInfo cameraInfo)
        {
            return new SoftwareCameraInfo
            {
                Platform = platform,
                Revision = cameraInfo.Revision
            };
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
