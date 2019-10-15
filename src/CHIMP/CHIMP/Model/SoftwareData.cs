using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;

namespace Chimp.Model
{
    sealed class SoftwareData : ISoftwareData
    {
        public SoftwareInfo Info { get; set; }
        public IMatchData Match { get; set; }
        public IDownloadData[] Downloads { get; set; }
        public string[] Paths { get; set; }
    }
}
