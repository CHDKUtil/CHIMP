using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using System;

namespace Net.Chdk.Providers.Camera
{
    sealed class EosProductCameraProvider : ProductCameraProvider<EosCameraData, EosCameraModelData, EosRevisionData, EosCardData>
    {
        public EosProductCameraProvider(string productName, ILoggerFactory loggerFactory)
            : base(productName, loggerFactory.CreateLogger<EosProductCameraProvider>())
        {
        }

        protected override string GetRevision(CameraInfo cameraInfo)
        {
            var version = cameraInfo.Canon.FirmwareVersion;
            return $"{version.Major}{version.Minor}{version.Build}";
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
