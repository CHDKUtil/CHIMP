using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Firmware
{
    public interface IFirmwareProvider
    {
        string? GetCategoryName(CameraInfo? cameraInfo);
        string? GetCategoryName(SoftwareCameraInfo? camera);
        string? GetFirmwareRevision(CameraInfo? cameraInfo, string? categoryName = null);
    }
}
