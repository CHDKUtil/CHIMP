using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class AltProvider : SingleProductProvider<IProductAltProvider>, IAltProvider
    {
        public AltProvider(IEnumerable<IProductAltProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public AltData GetAlt(string platform, TreeAltData tree, string productName)
        {
            return GetInnerProvider(productName).GetAlt(platform, tree);
        }
    }
}
