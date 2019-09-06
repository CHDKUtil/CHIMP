using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class RevisionProvider<TRevision> : SingleProductProvider<IProductRevisionProvider<TRevision>>, IRevisionProvider<TRevision>
        where TRevision : IRevisionData
    {
        public RevisionProvider(IEnumerable<IProductRevisionProvider<TRevision>> innerProviders)
            : base(innerProviders)
        {
        }

        public IDictionary<string, TRevision> GetRevisions(string productName, ListPlatformData list, TreePlatformData tree)
        {
            return GetInnerProvider(productName).GetRevisions(list, tree);
        }
    }
}
