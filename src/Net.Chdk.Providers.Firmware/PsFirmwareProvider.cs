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

        public string? GetRevisionString(string revision)
        {
            if (revision?.Length != 4)
                return null;
            return $"{revision[0]}.{revision[1]}{revision[2]}{char.ToUpper(revision[3])}";
        }
    }
}
