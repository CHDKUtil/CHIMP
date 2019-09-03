using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Camera.Fhp
{
    sealed class FhpCameraValidator : ProductCameraValidator
    {
        public FhpCameraValidator(ILogger<FhpCameraValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "400plus";
    }
}
