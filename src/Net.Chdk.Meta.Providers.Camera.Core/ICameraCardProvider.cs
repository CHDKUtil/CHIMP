using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface ICameraCardProvider<TCard>
        where TCard : CardData
    {
        TCard GetCard(uint modelId, bool multi, string productName);
    }
}
