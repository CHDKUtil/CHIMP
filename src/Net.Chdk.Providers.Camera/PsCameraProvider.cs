using System;
using Net.Chdk.Adapters.Platform;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;

namespace Net.Chdk.Providers.Camera
{
    sealed class PsCameraProvider : CategoryCameraProvider
    {
        public PsCameraProvider(IPlatformAdapter platformAdapter, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider)
            : base(platformAdapter, platformProvider, firmwareProvider)
        {
        }

        protected override string CategoryName => "PS";

        protected override uint GetFirmwareRevision(string revision)
        {
            return
                (uint)(revision[0] - 0x30 << 24) +
                (uint)((revision[1] - 0x30) << 20) +
                (uint)((revision[2] - 0x30) << 16) +
                (uint)((revision[3] - 0x60) << 8);
        }

        protected override Version? GetFirmwareVersion(string revision) => null;
    }
}
