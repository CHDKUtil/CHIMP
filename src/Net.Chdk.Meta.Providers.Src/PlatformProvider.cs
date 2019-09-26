using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class PlatformProvider<TPlatform, TRevision, TCamera, TData, T>
        where TPlatform : PlatformData<TPlatform, TRevision, PlatformSourceData>
        where TRevision : RevisionData<TRevision, PlatformSourceData>
        where TCamera : class
        where TData : class
        where T : struct
    {
        private RevisionProvider<TRevision, TData> RevisionProvider { get; }
        private CameraProvider<TCamera> CameraProvider { get; }
        private DataProvider<TData> DataProvider { get; }
        private ILogger Logger { get; }

        protected PlatformProvider(RevisionProvider<TRevision, TData> revisionProvider, CameraProvider<TCamera> cameraProvider, DataProvider<TData> dataProvider, ILogger logger)
        {
            RevisionProvider = revisionProvider;
            CameraProvider = cameraProvider;
            DataProvider = dataProvider;
            Logger = logger;
        }

        public TPlatform? GetPlatform(string platformPath, string platform)
        {
            Logger.LogTrace("Enter {0}", platform);

            var revisions = GetRevisions(platformPath, platform);
            var sourcePlatform = GetSourcePlatform(revisions, platform);
            var camera = GetCamera(platformPath, sourcePlatform ?? platform);
            var platformData = GetPlatformData(platformPath, platform, sourcePlatform, revisions, camera);
            if (platformData != null)
                platformData.Revisions = revisions;

            Logger.LogTrace("Leave {0}", platform);
            return platformData;
        }

        protected abstract TPlatform? GetPlatformData(string platformPath, string platform, string? sourcePlatform, IDictionary<string, TRevision> revisions, TCamera? camera);

        protected abstract T? GetValue(KeyValuePair<string, TRevision> revision);

        protected TData? GetData(string platformPath, string? sourcePlatform, string platform)
        {
            return DataProvider.GetData(platformPath, sourcePlatform ?? platform);
        }

        protected T? GetRevisionValue(IDictionary<string, TRevision> revisions, string platform)
        {
            var value = revisions
                .Select(GetValue)
                .FirstOrDefault(v => v != null);
            if (revisions.Any(kvp => !Equals(GetValue(kvp), value)))
                throw new InvalidOperationException($"{platform}: Mismatching data");
            return value;
        }

        private static string? GetSourcePlatform(IDictionary<string, TRevision> revisions, string platform)
        {
            var sourcePlatform = revisions.First().Value.Source?.Platform;
            if (revisions.Any(kvp => !Equals(kvp.Value.Source?.Platform, sourcePlatform)))
                throw new InvalidOperationException($"{platform}: Mismatching platforms");
            return sourcePlatform;
        }

        private TCamera? GetCamera(string platformPath, string platform)
        {
            return CameraProvider.GetCamera(platformPath, platform);
        }

        private IDictionary<string, TRevision> GetRevisions(string platformPath, string platform)
        {
            var subPath = Path.Combine(platformPath, platform, "sub");
            return Directory.EnumerateDirectories(subPath)
                .Select(Path.GetFileName)
                .ToDictionary(name => name, name => GetRevision(platformPath, platform, name));
        }

        private TRevision GetRevision(string platformPath, string platform, string revision)
        {
            return RevisionProvider.GetRevisionData(platformPath, platform, revision);
        }
    }
}
