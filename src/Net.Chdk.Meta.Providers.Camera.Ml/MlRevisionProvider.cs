using Net.Chdk.Meta.Providers.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Ml
{
    sealed class MlRevisionProvider : EosRevisionProvider
    {
        public override string ProductName => "ML";
    }
}