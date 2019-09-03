using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkCameraValidator : PsCameraValidator
    {
        public ChdkCameraValidator(ILogger<ChdkCameraValidator> logger)
            : base(logger)
        {
        }

        public override string ProductName => "CHDK";
    }
}
