using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using System;

namespace Net.Chdk.Providers.Camera
{
    sealed class PsProductCameraProvider : ProductCameraProvider<PsCameraData, PsCardData>
    {
        public PsProductCameraProvider(string productName, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(productName, firmwareProvider, loggerFactory.CreateLogger<PsProductCameraProvider>())
        {
        }

        protected override bool IsInvalid(CameraInfo cameraInfo)
        {
            return cameraInfo.Canon?.ModelId == null || cameraInfo.Canon?.FirmwareRevision == 0;
        }

        protected override bool IsMultiPartition(PsCameraData camera)
        {
            return camera.Card?.Multi == true;
        }

        protected override SoftwareEncodingInfo GetEncoding(PsCameraData camera)
        {
            return new SoftwareEncodingInfo
            {
                Name = camera.Encoding.Name,
                Data = camera.Encoding.Data,
            };
        }

        protected override AltInfo GetAlt(PsCameraData camera)
        {
            return new AltInfo
            {
                Button = camera.Alt.Button,
                Buttons = camera.Alt.Buttons,
            };
        }

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
