using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Camera
{
    public interface ICameraProvider
    {
        CameraModelsInfo GetCameraModels(CameraInfo cameraInfo);
        CameraModelsInfo GetCameraModels(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo);
        SoftwareCameraInfo GetCamera(string productName, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo);
        SoftwareEncodingInfo GetEncoding(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo);
        AltInfo GetAlt(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo);
    }
}
