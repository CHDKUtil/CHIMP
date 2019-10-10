using Net.Chdk.Model.Camera;

namespace Net.Chdk.Providers.Firmware
{
    sealed class EosFirmwareProvider : IInnerFirmwareProvider
    {
        public string? GetFirmwareRevision(CameraInfo? cameraInfo)
        {
            var version = cameraInfo?.Canon?.FirmwareVersion;
            if (version == null)
                return null;
            return $"{version.Major}{version.Minor}{version.Build}";
        }

        public string? GetRevisionString(string revision)
        {
            if (revision?.Length != 3)
                return null;
            return $"{revision[0]}.{revision[1]}.{revision[2]}";
        }
    }
}
