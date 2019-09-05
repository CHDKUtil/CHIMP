using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IProductCameraCardProvider<TCard> : IProductNameProvider
        where TCard : CardData
    {
        TCard GetCard(uint modelId, bool multi);
    }
}
