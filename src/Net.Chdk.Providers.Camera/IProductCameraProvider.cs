using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Camera
{
    interface IProductCameraProvider
    {
        CameraModelsInfo GetCameraModels(CameraInfo cameraInfo);
        CameraModelsInfo GetCameraModels(SoftwareCameraInfo cameraInfo);
        SoftwareCameraInfo GetCamera(CameraInfo cameraInfo, CameraModelInfo cameraModelInfo);
        SoftwareEncodingInfo GetEncoding(SoftwareCameraInfo cameraInfo);
        AltInfo GetAlt(SoftwareCameraInfo cameraInfo);
    }
}
