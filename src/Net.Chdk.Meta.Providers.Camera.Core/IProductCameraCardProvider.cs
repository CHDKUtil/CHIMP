using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IProductCameraCardProvider<TCard> : IProductNameProvider
        where TCard : CardData
    {
        TCard GetCard(uint modelId, TreeCardData card);
    }
}
