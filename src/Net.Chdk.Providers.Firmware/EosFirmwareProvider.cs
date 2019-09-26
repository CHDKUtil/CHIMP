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
    }
}
