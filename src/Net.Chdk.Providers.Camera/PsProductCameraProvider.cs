using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using System;

namespace Net.Chdk.Providers.Camera
{
    sealed class PsProductCameraProvider : ProductCameraProvider<PsCameraData, PsCardData>
    {
        public PsProductCameraProvider(string productName, ILoggerFactory loggerFactory)
            : base(productName, loggerFactory.CreateLogger<PsProductCameraProvider>())
        {
        }

        protected override string? GetRevision(CameraInfo cameraInfo)
        {
            return GetFirmwareRevision(cameraInfo.Canon!.FirmwareRevision);
        }

        protected override bool IsInvalid(CameraInfo? cameraInfo)
        {
            return cameraInfo?.Canon?.ModelId == null || cameraInfo.Canon?.FirmwareRevision == 0;
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

        protected override uint GetFirmwareRevision(string? revision)
        {
            if (revision == null)
                throw new ArgumentNullException(nameof(revision));
            return
                (uint)(revision[0] - 0x30 << 24) +
                (uint)((revision[1] - 0x30) << 20) +
                (uint)((revision[2] - 0x30) << 16) +
                (uint)((revision[3] - 0x60) << 8);
        }

        protected override Version? GetFirmwareVersion(string? revision) => null;

        private static string GetFirmwareRevision(uint revision)
        {
            return new string(new[] {
                (char)(((revision >> 24) & 0x0f) + 0x30),
                (char)(((revision >> 20) & 0x0f) + 0x30),
                (char)(((revision >> 16) & 0x0f) + 0x30),
                (char)(((revision >>  8) & 0x7f) + 0x60)
            });
        }
    }
}
