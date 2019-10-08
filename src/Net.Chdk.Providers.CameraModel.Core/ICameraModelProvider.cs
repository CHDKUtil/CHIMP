using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.CameraModel
{
    public interface ICameraModelProvider
    {
        (CameraInfo Info, CameraModelInfo[] Models)? GetCameraModels(CameraInfo cameraInfo);
        (CameraInfo Info, CameraModelInfo[] Models)? GetCameraModels(SoftwareCameraInfo? cameraInfo, SoftwareModelInfo? cameraModelInfo);
        (CameraInfo Info, CameraModelInfo[] Models)? GetCameraModels(SoftwareProductInfo? productInfo, SoftwareCameraInfo? cameraInfo);
        (SoftwareCameraInfo Camera, SoftwareModelInfo Model)? GetCameraModel(string productName, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo);
    }
}
