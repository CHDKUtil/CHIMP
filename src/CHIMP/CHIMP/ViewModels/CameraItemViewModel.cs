using Net.Chdk.Model.CameraModel;

namespace Chimp.ViewModels
{
    public sealed class CameraItemViewModel
    {
        public CameraItemViewModel(string displayName, CameraModelInfo model)
        {
            DisplayName = displayName;
            Model = model;
        }

        public string DisplayName { get; }
        public CameraModelInfo Model { get; }
    }
}
