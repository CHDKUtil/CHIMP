using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CameraProvider<TCamera, TModel, TRevision, TCard> : ICameraProvider<TCamera, TModel, TRevision, TCard>
        where TCamera : CameraData<TCamera, TModel, TRevision, TCard>, new()
        where TModel : CameraModelData<TModel, TRevision>
        where TRevision : IRevisionData
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
                Models = new TModel[0],
                Boot = GetBoot(modelId, productName),
                Card = GetCard(modelId, tree.MultiCard, productName),
            };
        }

        private BootData GetBoot(uint modelId, string productName)
        {
            return BootProvider.GetBoot(modelId, productName);
        }

        private TCard GetCard(uint modelId, bool multi, string productName)
        {
            return CardProvider.GetCard(modelId, multi, productName);
        }
    }
}
