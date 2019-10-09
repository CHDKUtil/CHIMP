using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Actions
{
    abstract class InstallActionBase : ActionBase
    {
        private SoftwareInfo Software { get; }
        private IDownloaderProvider DownloaderProvider { get; }
        private ProductSource ProductSource { get; }

        private string ProductName => ProductSource.ProductName;
        private string SourceName => ProductSource.SourceName;
        private SoftwareSourceInfo Source => ProductSource.Source;

        protected InstallActionBase(MainViewModel mainViewModel, IDownloaderProvider downloaderProvider, SoftwareInfo software, ProductSource productSource)
            : base(mainViewModel)
        {
            Software = software;
            DownloaderProvider = downloaderProvider;
            ProductSource = productSource;

            var distroName = string.Format("Distro_{0}", SourceName);
            var distroDisplayName = Resources.ResourceManager.GetString(distroName);
            DisplayName = string.Format(ActionFormat, distroDisplayName);
        }

        public override string DisplayName { get; }

        public override async Task<SoftwareData> PerformAsync(CancellationToken token)
        {
            return await GetDownloader().DownloadAsync(Software, token);
        }

        private IDownloader GetDownloader()
        {
            return DownloaderProvider.GetDownloader(ProductName, SourceName, Source);
        }

        protected abstract string ActionFormat { get; }
    }

    sealed class InstallAction : InstallActionBase
    {
        public InstallAction(MainViewModel mainViewModel, IDownloaderProvider downloaderProvider, SoftwareInfo software, ProductSource productSource)
            : base(mainViewModel, downloaderProvider, software, productSource)
        {
        }

        protected override string ActionFormat => Resources.Action_Install_Format;
    }

    sealed class UpdateAction : InstallActionBase
    {
        public UpdateAction(MainViewModel mainViewModel, IDownloaderProvider downloaderProvider, SoftwareInfo software, ProductSource productSource)
            : base(mainViewModel, downloaderProvider, software, productSource)
        {
        }

        public override bool IsDefault => true;

        protected override string ActionFormat => Resources.Action_Update_Format;
    }
}
