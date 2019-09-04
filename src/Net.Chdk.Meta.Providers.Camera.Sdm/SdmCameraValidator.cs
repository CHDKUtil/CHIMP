using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Sdm
{
    sealed class SdmCameraValidator : ProductCameraValidator
    {
        public SdmCameraValidator(ILogger<SdmCameraValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "SDM";

        protected override void OnListPlatformMissing(string platform)
        {
            Logger.LogWarning("{0} missing from list", platform);
        }
    }
}
