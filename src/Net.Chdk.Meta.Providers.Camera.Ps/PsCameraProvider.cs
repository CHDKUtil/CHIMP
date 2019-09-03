using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Providers.Product;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsCameraProvider : CameraProvider<PsCameraData, PsCameraModelData, PsCardData>, IPsCameraProvider
    {
        private IAltProvider AltProvider { get; }

        public PsCameraProvider(IProductProvider productProvider, IEncodingProvider encodingProvider, ICameraBootProvider bootProvider,
                ICameraCardProvider<PsCardData> cardProvider, IAltProvider altProvider)
            : base(productProvider, encodingProvider, bootProvider, cardProvider)
        {
            AltProvider = altProvider;
        }

        public override PsCameraData GetCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree, string productName)
        {
            var camera = base.GetCamera(modelId, platform, list, tree, productName);
            camera.Alt = GetAlt(platform, tree.Alt, productName);
            return camera;
        }

        private AltData GetAlt(string platform, TreeAltData alt, string productName)
        {
            return alt != null
                ? AltProvider.GetAlt(platform, alt, productName)
                : null;
        }
    }
}
