using Net.Chdk.Model.Camera;

namespace Net.Chdk.Providers.Firmware
{
    public interface IFirmwareProvider
    {
        string? GetCategoryName(CameraInfo? cameraInfo);
        string? GetFirmwareRevision(CameraInfo? cameraInfo, string? categoryName = null);
    }
}
