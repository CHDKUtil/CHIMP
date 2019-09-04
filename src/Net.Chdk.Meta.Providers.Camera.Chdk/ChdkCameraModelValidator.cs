using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkCameraModelValidator : ProductCameraModelValidator
    {
        public ChdkCameraModelValidator(ILogger<ChdkCameraModelValidator> logger)
            : base(logger)
        {
        }

        protected override void OnTreeRevisionMissing(string platform, string revision)
        {
            Logger.LogWarning("{0}: {1} missing from tree", platform, revision);
        }

        public override string ProductName => "CHDK";
    }
}
