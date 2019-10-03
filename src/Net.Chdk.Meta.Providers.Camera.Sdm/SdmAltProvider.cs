using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Sdm
{
    sealed class SdmAltProvider : ProductAltProvider
    {
        public SdmAltProvider(ILogger<SdmAltProvider> logger)
            : base(logger)
        {
        }

        public override string ProductName => "SDM";

        protected override string GetAltButton(string platform, string[]? altNames)
        {
            return "Menu";
        }

        protected override string[]? GetAltButtons(string platform, string[]? altNames)
        {
            return null;
        }
    }
}
