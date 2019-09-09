using Net.Chdk.Model.Camera;

namespace Net.Chdk.Detectors.Camera
{
    public interface IFileCameraDetector
    {
        CameraInfo GetCamera(string filePath);
    }
}
