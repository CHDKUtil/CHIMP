using Net.Chdk.Meta.Model.Camera.Ps;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public abstract class PsCameraCardProvider : ProductCameraCardProvider<PsCardData>
    {
        private const uint MinModelId = 0x1010000;
        private const uint MinFat32ModelId = 0x2980000;
        private const uint MinSdhcModelId = 0x2000000;
        private const uint MinSdxcModelId = 0x2800000;

        private static readonly uint[] MicroSdModelIds = { 0x3240000, 0x3250000, 0x3380000 };
        private static readonly uint[] SdhcModelIds = { 0x1950000, 0x1980000 };

        public override PsCardData GetCard(uint modelId, bool multi)
        {
            var cardData = base.GetCard(modelId, multi);
            cardData.Multi = IsMultiCard(modelId, multi);
            return cardData;
        }

        protected override string GetCardType(uint modelId)
        {
            return MicroSdModelIds.Contains(modelId)
                ? "microSD"
                : "SD";
        }

        protected override string GetCardSubtype(uint modelId)
        {
            if (modelId < MinModelId)
                return "SDXC";
            if (modelId < MinSdhcModelId && !SdhcModelIds.Contains(modelId))
                return "SDSC";
            if (modelId < MinSdxcModelId)
                return "SDHC";
            return "SDXC";
        }

        private static bool IsMultiCard(uint modelId, bool multi)
        {
            return multi && modelId >= MinModelId && modelId < MinFat32ModelId;
        }
    }
}
