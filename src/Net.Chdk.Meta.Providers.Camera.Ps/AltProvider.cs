using Net.Chdk.Meta.Model.Camera.Ps;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class AltProvider : SingleProductProvider<IProductAltProvider>, IAltProvider
    {
        public AltProvider(IEnumerable<IProductAltProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public AltData GetAlt(string platform, string[]? altNames, string productName)
        {
            return GetInnerProvider(productName).GetAlt(platform, altNames);
        }
    }
}
