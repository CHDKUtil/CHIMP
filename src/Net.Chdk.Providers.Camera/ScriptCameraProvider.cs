using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using System;

namespace Net.Chdk.Providers.Camera
{
    sealed class ScriptCameraData : CameraData<ScriptCameraData, CardData>
    {
    }

    sealed class ScriptCameraProvider : ProductCameraProvider<ScriptCameraData, CardData>
    {
        public ScriptCameraProvider(string productName, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(productName, firmwareProvider, loggerFactory.CreateLogger<ScriptCameraProvider>())
        {
        }

        protected override bool IsInvalid(CameraInfo cameraInfo)
        {
            return cameraInfo.Canon?.ModelId == null || cameraInfo.Canon?.FirmwareRevision == 0;
        }

        protected override bool IsMultiPartition(ScriptCameraData camera) => false;

        protected override SoftwareEncodingInfo GetEncoding(ScriptCameraData camera) => null;

        protected override AltInfo GetAlt(ScriptCameraData camera) => null;

        protected override uint GetFirmwareRevision(string revision)
        {
            return
                (uint)(revision[0] - 0x30 << 24) +
                (uint)((revision[1] - 0x30) << 20) +
                (uint)((revision[2] - 0x30) << 16) +
                (uint)((revision[3] - 0x60) << 8);
        }

        protected override Version GetFirmwareVersion(string revision) => null;
    }
}