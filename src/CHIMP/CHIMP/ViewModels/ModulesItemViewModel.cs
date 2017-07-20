using Net.Chdk.Model.Software;

namespace Chimp.ViewModels
{
    sealed class ModulesItemViewModel
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public ModuleInfo Info { get; set; }
        public string ToolTip { get; set; }
    }
}
