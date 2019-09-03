using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public abstract class ProductRevisionProvider : IProductRevisionProvider
    {
        public IDictionary<string, RevisionData> GetRevisions(ListPlatformData list, TreePlatformData tree)
        {
            var revisions = new SortedDictionary<string, RevisionData>();
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

        protected abstract RevisionData GetRevision(string revision, ListRevisionData listRevision, ListPlatformData list);

        public abstract string ProductName { get; }

        protected static RevisionData GetRevision(string revision)
        {
            return new RevisionData
            {
                Revision = revision,
            };
        }

        private static string GetRevisionKey(string revisionStr)
        {
            var revision = GetFirmwareRevision(revisionStr);
            return $"0x{revision:x}";
        }

        private static uint GetFirmwareRevision(string revision)
        {
            if (revision == null)
                return 0;
            return
                (uint)((revision[0] - 0x30) << 24) +
                (uint)((revision[1] - 0x30) << 20) +
                (uint)((revision[2] - 0x30) << 16) +
                (uint)((revision[3] - 0x60) << 8);
        }
    }
}
