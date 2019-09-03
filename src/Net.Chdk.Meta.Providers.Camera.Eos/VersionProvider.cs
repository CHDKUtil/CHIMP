using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class VersionProvider : IVersionProvider
    {
        public IDictionary<string, VersionData> GetVersions(string productName, ListPlatformData list, TreePlatformData tree)
        {
            var versions = new SortedDictionary<string, VersionData>();
            foreach (var kvp in list.Revisions)
            {
                var version = GetVersion(kvp.Key, kvp.Value, list, tree);
                if (version != null)
                {
                    var versionKey = GetVersionKey(kvp.Key);
                    versions.Add(versionKey, version);
                }
            }
            return versions;
        }

        private VersionData GetVersion(string version, ListRevisionData listRevision, ListPlatformData list, TreePlatformData tree)
        {
            var key = listRevision.Source?.Revision ?? version;
            if (!list.Revisions.ContainsKey(key))
                return null;

            return GetVersion(version);
        }

        private static string GetVersionKey(string version)
        {
            return $"{version[0]}.{version[1]}.{version[2]}";
        }

        private static VersionData GetVersion(string version)
        {
            return new VersionData
            {
                Version = version
            };
        }
    }
}
