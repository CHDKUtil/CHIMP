using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CameraProvider<TCamera, TCard> : ICameraProvider<TCamera, TCard>
        where TCamera : CameraData<TCamera, TCard>, new()
        where TCard : CardData
    {
        private ICameraBootProvider BootProvider { get; }
        private ICameraCardProvider<TCard> CardProvider { get; }

        protected CameraProvider(ICameraBootProvider bootProvider, ICameraCardProvider<TCard> cardProvider)
        {
            BootProvider = bootProvider;
            CardProvider = cardProvider;
        }

        public virtual TCamera GetCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree, string productName)
        {
            return new TCamera
            {
                Models = new CameraModelData[0],
                Boot = GetBoot(modelId, tree.MultiCard, productName),
                Card = GetCard(modelId, tree.MultiCard, productName),
            };
        }

        private BootData GetBoot(uint modelId, bool multi, string productName)
        {
            return BootProvider.GetBoot(modelId, multi, productName);
        }

        private TCard GetCard(uint modelId, bool multi, string productName)
        {
            return CardProvider.GetCard(modelId, multi, productName);
        }
    }
}
