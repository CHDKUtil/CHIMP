using Net.Chdk.Model.Software;

namespace Chimp.Model
{
    sealed class SoftwareData
    {
        public SoftwareInfo Info { get; set; }
        public MatchData Match { get; set; }
        public DownloadData[] Downloads { get; set; }
        public string[] Paths { get; set; }
    }
}
