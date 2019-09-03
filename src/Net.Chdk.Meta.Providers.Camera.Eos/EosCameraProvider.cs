using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Providers.Product;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosCameraProvider : CameraProvider<EosCameraData, EosCameraModelData, EosCardData>, IEosCameraProvider
    {
        public EosCameraProvider(IProductProvider productProvider, IEncodingProvider encodingProvider, ICameraBootProvider bootProvider, ICameraCardProvider<EosCardData> cardProvider)
            : base(productProvider, encodingProvider, bootProvider, cardProvider)
        {
        }
    }
}
