using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using System;

namespace Net.Chdk.Providers.Camera
{
    sealed class EosCameraProvider : CategoryCameraProvider
    {
        public EosCameraProvider(IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider)
            : base(platformProvider, firmwareProvider)
        {
        }

        protected override string CategoryName => "EOS";

        protected override uint GetFirmwareRevision(string revision) => 0;

        protected override Version GetFirmwareVersion(string revision)
        {
            string version = $"{revision[0]}.{revision[1]}.{revision[2]}";
            return Version.Parse(version);
        }
    }
}
