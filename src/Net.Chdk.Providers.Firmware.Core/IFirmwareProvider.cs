using Net.Chdk.Model.Camera;

namespace Net.Chdk.Providers.Firmware
{
    public interface IFirmwareProvider
    {
        string? GetCategoryName(CameraInfo? cameraInfo);
        public string? GetFirmwareRevision(CameraInfo? cameraInfo);
    }
}
