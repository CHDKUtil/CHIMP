using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Providers.Product;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CameraProvider<TCamera, TModel, TCard>
        where TCamera : CameraData<TCamera, TModel, TCard>, new()
        where TModel : CameraModelData
        where TCard : CardData
    {
        private IProductProvider ProductProvider { get; }
        private IEncodingProvider EncodingProvider { get; }
        private ICameraBootProvider BootProvider { get; }
        private ICameraCardProvider<TCard> CardProvider { get; }

        protected CameraProvider(IProductProvider productProvider, IEncodingProvider encodingProvider, ICameraBootProvider bootProvider, ICameraCardProvider<TCard> cardProvider)
        {
            ProductProvider = productProvider;
            EncodingProvider = encodingProvider;
            BootProvider = bootProvider;
            CardProvider = cardProvider;
        }

        public virtual TCamera GetCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree, string productName)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            return new TCamera
            {
                Models = new TModel[0],
                Encoding = GetEncoding(platform, tree.Encoding, categoryName),
                Boot = GetBoot(modelId, productName),
                Card = GetCard(modelId, tree.MultiCard, productName),
            };
        }

        private EncodingData GetEncoding(string platform, byte? encoding, string categoryName)
        {
            return encoding != null
                ? EncodingProvider.GetEncoding(platform, encoding.Value, categoryName)
                : null;
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
