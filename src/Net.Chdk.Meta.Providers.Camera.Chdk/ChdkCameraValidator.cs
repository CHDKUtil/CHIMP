using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkCameraValidator : ProductCameraValidator
    {
        public ChdkCameraValidator(ILogger<ChdkCameraValidator> logger)
            : base(logger)
        {
        }

        protected override void OnListPlatformMissing(string platform)
        {
            Logger.LogWarning("{0} missing from list", platform);
        }

        public override string ProductName => "CHDK";
    }
}
