using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public abstract class PsCameraValidator : ProductCameraValidator
    {
        protected PsCameraValidator(ILogger logger)
            : base(logger)
        {
        }

        protected override void OnListPlatformMissing(string platform)
        {
            Logger.LogWarning("{0} missing from list", platform);
        }
    }
}
