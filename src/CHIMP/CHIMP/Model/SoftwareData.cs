using Net.Chdk.Model.Software;

namespace Chimp.Model
{
    sealed class SoftwareData
    {
        public SoftwareInfo Info { get; set; }
        public string[] Paths { get; set; }
    }
}
