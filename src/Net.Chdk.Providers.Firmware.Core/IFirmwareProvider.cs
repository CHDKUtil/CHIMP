using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;

namespace Net.Chdk.Providers.Firmware
{
    public interface IFirmwareProvider
    {
        string? GetCategoryName(CameraInfo? cameraInfo);
        string? GetFirmwareRevision(CameraInfo? cameraInfo, string? categoryName = null);
        string? GetRevisionString(string revision, string categoryName);
        string? GetModelName(CameraInfo camera, CameraModelInfo cameraModel);
    }
}
