using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class RevisionProvider : SingleProductProvider<IProductRevisionProvider>, IRevisionProvider
    {
        public RevisionProvider(IEnumerable<IProductRevisionProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public IDictionary<string, RevisionData> GetRevisions(string productName, ListPlatformData list, TreePlatformData tree)
        {
            return GetInnerProvider(productName).GetRevisions(list, tree);
        }
    }
}
