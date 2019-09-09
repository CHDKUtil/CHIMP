using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsCameraProvider : CameraProvider<PsCameraData, PsCameraModelData, RevisionData, PsCardData>
    {
        private IEncodingProvider EncodingProvider { get; }
        private IAltProvider AltProvider { get; }

        public PsCameraProvider(IEncodingProvider encodingProvider, ICameraBootProvider bootProvider, ICameraCardProvider<PsCardData> cardProvider,
            IAltProvider altProvider)
            : base(bootProvider, cardProvider)
        {
            EncodingProvider = encodingProvider;
            AltProvider = altProvider;
        }

        public override PsCameraData GetCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree, string productName)
        {
            var camera = base.GetCamera(modelId, platform, list, tree, productName);
            camera.Encoding = GetEncoding(platform, tree.Encoding);
            camera.Alt = GetAlt(platform, tree.AltNames, productName);
            return camera;
        }

        private EncodingData GetEncoding(string platform, byte? encoding)
        {
            return encoding != null
                ? EncodingProvider.GetEncoding(platform, encoding.Value)
                : null;
        }

        private AltData GetAlt(string platform, string[] altNames, string productName)
        {
            return AltProvider.GetAlt(platform, altNames, productName);
        }
    }
}
