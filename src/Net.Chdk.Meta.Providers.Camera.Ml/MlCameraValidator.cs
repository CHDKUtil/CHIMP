using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Ml
{
    sealed class MlCameraValidator : ProductCameraValidator
    {
        public MlCameraValidator(ILogger<MlCameraValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "ML";
    }
}
