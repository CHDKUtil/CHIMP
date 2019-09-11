using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosCameraProvider : CameraProvider<EosCameraData, EosCardData>, ICameraProvider<EosCameraData, EosCardData>
    {
        public EosCameraProvider(ICameraBootProvider bootProvider, ICameraCardProvider<EosCardData> cardProvider)
            : base(bootProvider, cardProvider)
        {
        }
    }
}
