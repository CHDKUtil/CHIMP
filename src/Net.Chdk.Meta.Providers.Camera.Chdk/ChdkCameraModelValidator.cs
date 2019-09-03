using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkCameraModelValidator : ProductCameraModelValidator
    {
        public ChdkCameraModelValidator(ILogger<ChdkCameraModelValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "CHDK";
    }
}
