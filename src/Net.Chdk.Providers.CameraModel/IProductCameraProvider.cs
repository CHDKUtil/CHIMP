using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.CameraModel
{
    interface IProductCameraProvider
    {
        SoftwareEncodingInfo? GetEncoding(SoftwareCameraInfo cameraInfo);
        string? GetAltButton(SoftwareCameraInfo cameraInfo);
        string? GetCardType(CameraInfo cameraInfo);
        string? GetCardSubtype(CameraInfo cameraInfo);
        string? GetBootFileSystem(CameraInfo cameraInfo);
        bool IsMultiPartition(CameraInfo cameraInfo);
    }
}
