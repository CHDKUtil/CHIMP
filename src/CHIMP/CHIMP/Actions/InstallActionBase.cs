using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Actions
{
    abstract class InstallActionBase : ActionBase
    {
        private ICameraProvider CameraProvider { get; }
        private IDownloaderProvider DownloaderProvider { get; }
        private ProductSource ProductSource { get; }

        private string ProductName => ProductSource.ProductName;
        private string SourceName => ProductSource.SourceName;
        private SoftwareSourceInfo Source => ProductSource.Source;

        protected InstallActionBase(MainViewModel mainViewModel, ICameraProvider cameraProvider, IDownloaderProvider downloaderProvider, ProductSource productSource)
            : base(mainViewModel)
        {
            CameraProvider = cameraProvider;
            DownloaderProvider = downloaderProvider;
            ProductSource = productSource;

            var distroName = string.Format("Distro_{0}", SourceName);
            var distroDisplayName = Resources.ResourceManager.GetString(distroName);
            DisplayName = string.Format(ActionFormat, distroDisplayName);
        }

        public override string DisplayName { get; }

        public override async Task<SoftwareData> PerformAsync(CancellationToken token)
        {
            var software = SoftwareViewModel.SelectedItem?.Info;
            var camera = CameraProvider.GetCamera(ProductName, CameraViewModel.Info, CameraViewModel.SelectedItem.Model);
            if (camera == null)
            {
                DownloadViewModel.Title = nameof(Resources.Download_UnsupportedModel_Text);
                return null;
            }
            if (camera.Revision == null)
            {
                DownloadViewModel.Title = nameof(Resources.Download_UnsupportedFirmware_Text);
                return null;
            }
            return await GetDownloader().DownloadAsync(camera, software, token);
        }

        private IDownloader GetDownloader()
        {
            return DownloaderProvider.GetDownloader(ProductName, SourceName, Source);
        }

        protected abstract string ActionFormat { get; }
    }

    sealed class InstallAction : InstallActionBase
    {
        public InstallAction(MainViewModel mainViewModel, ICameraProvider cameraProvider, IDownloaderProvider downloaderProvider, ProductSource productSource)
            : base(mainViewModel, cameraProvider, downloaderProvider, productSource)
        {
        }

        protected override string ActionFormat => Resources.Action_Install_Format;
    }

    sealed class UpdateAction : InstallActionBase
    {
        public UpdateAction(MainViewModel mainViewModel, ICameraProvider cameraProvider, IDownloaderProvider downloaderProvider, ProductSource productSource)
            : base(mainViewModel, cameraProvider, downloaderProvider, productSource)
        {
        }

        public override bool IsDefault => true;

        protected override string ActionFormat => Resources.Action_Update_Format;
    }
}
