using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Camera
{
    interface ICategoryCameraProvider
    {
        CameraModelsInfo? GetCameraModels(CameraInfo cameraInfo);
        CameraModelsInfo? GetCameraModels(SoftwareCameraInfo? cameraInfo);
        SoftwareCameraInfo? GetCamera(CameraInfo cameraInfo, CameraModelInfo cameraModelInfo);
    }
}
