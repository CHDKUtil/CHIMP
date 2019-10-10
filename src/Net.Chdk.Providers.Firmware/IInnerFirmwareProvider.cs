using Net.Chdk.Model.Camera;

namespace Net.Chdk.Providers.Firmware
{
    interface IInnerFirmwareProvider
    {
        string? GetFirmwareRevision(CameraInfo? cameraInfo);
        string? GetRevisionString(string revision);
    }
}