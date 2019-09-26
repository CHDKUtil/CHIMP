using Net.Chdk.Model.Camera;

namespace Net.Chdk.Providers.Firmware
{
    sealed class PsFirmwareProvider : IInnerFirmwareProvider
    {
        public string? GetFirmwareRevision(CameraInfo? cameraInfo)
        {
            var canon = cameraInfo?.Canon;
            if (canon == null)
                return null;
            var revision = canon.FirmwareRevision;
            return new string(new[] {
                (char)(((revision >> 24) & 0x0f) + 0x30),
                (char)(((revision >> 20) & 0x0f) + 0x30),
                (char)(((revision >> 16) & 0x0f) + 0x30),
                (char)(((revision >>  8) & 0x7f) + 0x60)
            });
        }
    }
}
