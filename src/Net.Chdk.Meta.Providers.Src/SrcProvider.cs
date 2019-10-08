using Net.Chdk.Meta.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class SrcProvider<TPlatform, TRevision, TCamera, TData, T>
        where TPlatform : PlatformData<TPlatform, TRevision, PlatformSourceData>
        where TRevision : RevisionData<TRevision, PlatformSourceData>
        where TCamera : class
        where TData : class
        where T : struct
    {
        private PlatformProvider<TPlatform, TRevision, TCamera, TData, T> PlatformProvider { get; }

        protected SrcProvider(PlatformProvider<TPlatform, TRevision, TCamera, TData, T> platformProvider)
        {
            PlatformProvider = platformProvider;
        }

        public string Extension => string.Empty;

        protected IDictionary<string, TPlatform?> GetTree(string path)
        {
            var platformPath = Path.Combine(path, "platform");
            return Directory.EnumerateDirectories(platformPath)
                .Select(Path.GetFileName)
                .Where(platform => !"generic".Equals(platform, StringComparison.Ordinal))
                .ToDictionary(platform => platform, platform => GetPlatform(platformPath, platform));
        }

        private TPlatform? GetPlatform(string platformPath, string platform)
        {
            return PlatformProvider.GetPlatform(platformPath, platform);
        }
    }
}
