using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class ProductCameraCardProvider<TCard> : IProductCameraCardProvider<TCard>
        where TCard : CardData, new()
    {
        public virtual TCard GetCard(uint modelId, bool multi)
        {
            return new TCard
            {
                Type = GetCardType(modelId),
                Subtype = GetCardSubtype(modelId),
            };
        }

        public abstract string ProductName { get; }

        protected abstract string GetCardType(uint modelId);
        protected abstract string? GetCardSubtype(uint modelId);
    }
}
