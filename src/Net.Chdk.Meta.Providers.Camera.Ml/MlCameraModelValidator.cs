using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Ml
{
    sealed class MlCameraModelValidator : ProductCameraModelValidator
    {
        public MlCameraModelValidator(ILogger<MlCameraModelValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "ML";
    }
}
