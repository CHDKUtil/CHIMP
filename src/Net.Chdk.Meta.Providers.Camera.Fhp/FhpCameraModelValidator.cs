using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Fhp
{
    sealed class FhpCameraModelValidator : ProductCameraModelValidator
    {
        public FhpCameraModelValidator(ILogger<FhpCameraModelValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "400plus";
    }
}
