using Net.Chdk.Meta.Providers.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Ml
{
    sealed class MlCameraCardProvider : EosCameraCardProvider
    {
        public override string ProductName => "ML";

        protected override string GetCardType(uint modelId) => "SD";
    }
}
