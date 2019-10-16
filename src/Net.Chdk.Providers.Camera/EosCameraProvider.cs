using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Camera
{
    sealed class EosCameraProvider : ProductCameraProvider<EosCameraData, EosCardData>
    {
        public EosCameraProvider(string productName, ILoggerFactory loggerFactory)
            : base(productName, loggerFactory.CreateLogger<EosCameraProvider>())
        {
        }

        protected override bool IsInvalid(CameraInfo cameraInfo)
        {
            return cameraInfo?.Canon?.ModelId == null || cameraInfo?.Canon?.FirmwareVersion == null;
        }

        public override SoftwareEncodingInfo? GetEncoding(SoftwareCameraInfo _)
        {
            return SoftwareEncodingInfo.Empty;
        }

        public override string? GetAltButton(SoftwareCameraInfo _)
        {
            return null;
        }

        protected override bool IsMultiPartition(EosCameraData? _)
        {
            return false;
        }
    }
}