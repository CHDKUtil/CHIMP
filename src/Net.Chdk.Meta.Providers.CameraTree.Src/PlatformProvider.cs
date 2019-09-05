using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class PlatformProvider
    {
        private ILogger Logger { get; }
        private RevisionProvider RevisionProvider { get; }
        private DataProvider DataProvider { get; }
        private CameraProvider CameraProvider { get; }

        public PlatformProvider(RevisionProvider revisionProvider, DataProvider dataProvider, CameraProvider cameraProvider, ILogger<PlatformProvider> logger)
        {
            RevisionProvider = revisionProvider;
            DataProvider = dataProvider;
            CameraProvider = cameraProvider;
            Logger = logger;
        }

        public TreePlatformData GetPlatform(string platformPath, string platform)
        {
            Logger.LogTrace("Enter {0}", platform);

            var revisions = GetRevisions(platformPath, platform);
            var sourcePlatform = GetSourcePlatform(revisions, platform);
            var data = GetData(platformPath, sourcePlatform, platform);
            var id = GetId(data, platform, revisions);
            var encoding = GetEncoding(data, platform, revisions);
            var camera = GetCamera(platformPath, sourcePlatform ?? platform);

            var platformData = new TreePlatformData
            {
                Revisions = revisions,
                Id = id,
                Encoding = encoding,
                Alt = GetAlt(camera),
                MultiCard = IsMultiCard(camera),
            };

            Logger.LogTrace("Leave {0}", platform);
            return platformData;
        }

        private IDictionary<string, TreeRevisionData> GetRevisions(string platformPath, string platform)
        {
            var subPath = Path.Combine(platformPath, platform, "sub");
            return Directory.EnumerateDirectories(subPath)
                .Select(Path.GetFileName)
                .ToDictionary(name => name, name => GetRevision(platformPath, platform, name));
        }

        private TreeRevisionData GetRevision(string platformPath, string platform, string revision)
        {
            return RevisionProvider.GetRevisionData(platformPath, platform, revision);
        }

        private string GetSourcePlatform(IDictionary<string, TreeRevisionData> revisions, string platform)
        {
            var sourcePlatform = revisions.First().Value.Source?.Platform;
            if (revisions.Any(kvp => !Equals(kvp.Value.Source?.Platform, sourcePlatform)))
                throw new InvalidOperationException($"{platform}: Mismatching platforms");
            return sourcePlatform;
        }

        private CameraData GetCamera(string platformPath, string platform)
        {
            return CameraProvider.GetCamera(platformPath, platform);
        }

        private static TreeAltData GetAlt(CameraData camera)
        {
            return camera != null
                ? camera.Alt
                : new TreeAltData();
        }

        private static bool IsMultiCard(CameraData camera)
        {
            return camera?.MultiCard == true;
        }

        private RevisionData GetData(string platformPath, string sourcePlatform, string platform)
        {
            return DataProvider.GetData(platformPath, sourcePlatform ?? platform);
        }

        private static ushort GetId(RevisionData data, string platform, IDictionary<string, TreeRevisionData> revisions)
        {
            return data?.Id
                ?? GetRevisionId(revisions, platform)
                ?? throw new InvalidOperationException($"{platform}: Missing ID");
        }

        private static byte GetEncoding(RevisionData data, string platform, IDictionary<string, TreeRevisionData> revisions)
        {
            return data?.Encoding
                ?? GetRevisionEncoding(revisions, platform)
                ?? 0;
        }

        private static ushort? GetRevisionId(IDictionary<string, TreeRevisionData> revisions, string platform)
        {
            var id = revisions
                .Select(kvp => kvp.Value.Id)
                .FirstOrDefault(i => i != null);
            if (revisions.Any(kvp => !Equals(kvp.Value.Id, id)))
                throw new InvalidOperationException($"{platform}: Mismatching IDs");
            return id;
        }

        private static byte? GetRevisionEncoding(IDictionary<string, TreeRevisionData> revisions, string platform)
        {
            var encoding = revisions
                .Select(kvp => kvp.Value.Encoding)
                .FirstOrDefault(e => e != null);
            if (revisions.Any(kvp => !Equals(kvp.Value.Encoding, encoding)))
                throw new InvalidOperationException($"{platform}: Mismatching encodings");
            return encoding;
        }
    }
}
