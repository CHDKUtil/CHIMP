using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using System;

namespace Net.Chdk.Providers.Camera
{
    sealed class EosProductCameraProvider : ProductCameraProvider<EosCameraData, EosCardData>
    {
        public EosProductCameraProvider(string productName, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(productName, firmwareProvider, loggerFactory.CreateLogger<EosProductCameraProvider>())
        {
        }

        protected override bool IsInvalid(CameraInfo cameraInfo)
        {
            return cameraInfo.Canon?.ModelId == null || cameraInfo.Canon?.FirmwareVersion == null;
        }

        protected override bool IsMultiPartition(EosCameraData camera)
        {
            return false;
        }

        protected override SoftwareEncodingInfo GetEncoding(EosCameraData camera)
        {
            return SoftwareEncodingInfo.Empty;
        }

        protected override AltInfo GetAlt(EosCameraData camera)
        {
            return AltInfo.Empty;
        }

        protected override uint GetFirmwareRevision(string revision) => 0;

        protected override Version GetFirmwareVersion(string revision)
        {
            string version = $"{revision[0]}.{revision[1]}.{revision[2]}";
            return Version.Parse(version);
        }
    }
}
