using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkCameraValidator : ProductCameraValidator
    {
        public ChdkCameraValidator(ILogger<ChdkCameraValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "CHDK";
    }
}
