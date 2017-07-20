using Net.Chdk.Model.CameraModel;

namespace Chimp.ViewModels
{
    public sealed class CameraItemViewModel
    {
        public string DisplayName { get; set; }
        public CameraModelInfo Model { get; set; }
    }
}
