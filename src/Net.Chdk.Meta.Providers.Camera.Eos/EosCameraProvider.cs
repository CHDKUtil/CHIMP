using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosCameraProvider : CameraProvider<EosCameraData, EosCameraModelData, EosRevisionData, EosCardData>, ICameraProvider<EosCameraData, EosCameraModelData, EosRevisionData, EosCardData>
    {
        public EosCameraProvider(ICameraBootProvider bootProvider, ICameraCardProvider<EosCardData> cardProvider)
            : base(bootProvider, cardProvider)
        {
        }
    }
}
