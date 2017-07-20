using Net.Chdk.Model.Software;

namespace Chimp.ViewModels
{
    sealed class SoftwareItemViewModel
    {
        public string DisplayName { get; set; }
        public SoftwareInfo Info { get; set; }
        public ModulesViewModel Modules { get; set; }
    }
}
