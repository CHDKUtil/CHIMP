using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Sdm
{
    sealed class SdmCameraValidator : PsCameraValidator
    {
        public SdmCameraValidator(ILogger<SdmCameraValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "SDM";
    }
}
