using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;

namespace Chimp.Model
{
    sealed class DownloadsData<TDownloadData>
        where TDownloadData : IDownloadData
    {
        public SoftwareInfo Info { get; set; }
        public TDownloadData[] Downloads { get; set; }
    }
}
