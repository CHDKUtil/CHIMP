using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraCardProvider<TCard> : SingleProductProvider<IProductCameraCardProvider<TCard>>, ICameraCardProvider<TCard>
        where TCard : CardData
    {
        public CameraCardProvider(IEnumerable<IProductCameraCardProvider<TCard>> innerProviders)
            : base(innerProviders)
        {
        }

        public TCard GetCard(uint modelId, TreeCardData card, string productName)
        {
            return GetInnerProvider(productName).GetCard(modelId, card);
        }
    }
}
