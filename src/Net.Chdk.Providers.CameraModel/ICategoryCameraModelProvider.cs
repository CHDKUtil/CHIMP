using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.CameraModel
{
    interface ICategoryCameraModelProvider
    {
        (CameraInfo, CameraModelInfo[])? GetCameraModels(CameraInfo cameraInfo);
        (CameraInfo, CameraModelInfo[])? GetCameraModels(SoftwareCameraInfo? cameraInfo, SoftwareModelInfo? cameraModelInfo);
        (CameraInfo, CameraModelInfo[])? GetCameraModels(SoftwareProductInfo? productInfo, SoftwareCameraInfo? cameraInfo);
        (SoftwareCameraInfo, SoftwareModelInfo)? GetCameraModel(CameraInfo cameraInfo, CameraModelInfo cameraModelInfo);
    }
}
