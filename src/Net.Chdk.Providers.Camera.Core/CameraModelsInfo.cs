using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;

namespace Net.Chdk.Providers.Camera
{
    public sealed class CameraModelsInfo
    {
        public CameraInfo? Info { get; set; }
        public CameraModelInfo[]? Models { get; set; }
    }
}
