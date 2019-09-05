using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class ProductRevisionProvider<TRevision> : IProductRevisionProvider
        where TRevision : IRevisionData, new()
    {
        public IDictionary<string, IRevisionData> GetRevisions(ListPlatformData list, TreePlatformData tree)
        {
            var revisions = new SortedDictionary<string, IRevisionData>();
            foreach (var kvp in list.Revisions)
            {
                var revision = GetRevision(kvp.Key, kvp.Value, list);
                if (revision != null)
                {
                    var revisionKey = GetRevisionKey(kvp.Key);
                    revisions.Add(revisionKey, revision);
                }
            }
            return revisions;
        }

        protected virtual IRevisionData GetRevision(string revision, ListRevisionData listRevision, ListPlatformData list)
        {
            var key = listRevision.Source?.Revision ?? revision;
            if (!list.Revisions.ContainsKey(key))
                return null;

            return GetRevision(key);
        }

        protected IRevisionData GetRevision(string key)
        {
            return new TRevision
            {
                Revision = key
            };
        }

        protected abstract string GetRevisionKey(string revison);

        public abstract string ProductName { get; }
    }
}
