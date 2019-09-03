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
        private IdProvider IdProvider { get; }
        private EncodingProvider EncodingProvider { get; }
        private CameraProvider CameraProvider { get; }

        public PlatformProvider(RevisionProvider revisionProvider, IdProvider idProvider, EncodingProvider encodingProvider, CameraProvider cameraProvider, ILogger<PlatformProvider> logger)
        {
            RevisionProvider = revisionProvider;
            IdProvider = idProvider;
            EncodingProvider = encodingProvider;
            CameraProvider = cameraProvider;
            Logger = logger;
        }

        public TreePlatformData GetPlatform(string platformPath, string platform)
        {
            Logger.LogTrace("Enter {0}", platform);

            var revisions = GetRevisions(platformPath, platform);
            var sourcePlatform = GetSourcePlatform(revisions, platform);
            var id = GetId(platformPath, sourcePlatform, platform, revisions);
            var encoding = GetEncoding(platformPath, sourcePlatform, platform, revisions);
            var camera = GetCamera(platformPath, sourcePlatform ?? platform);

            var platformData = new TreePlatformData
            {
                Revisions = revisions,
                Id = id,
                Encoding = encoding,
                Alt = GetAlt(camera),
                Card = GetCard(camera),
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

        private TreeIdData GetId(string platformPath, string sourcePlatform, string platform, IDictionary<string, TreeRevisionData> revisions)
        {
            return GetPlatformId(platformPath, sourcePlatform, platform)
                ?? GetRevisionId(revisions, platform);
        }

        private TreeIdData GetPlatformId(string platformPath, string sourcePlatform, string platform)
        {
            return IdProvider.GetId(platformPath, sourcePlatform ?? platform);
        }

        private TreeIdData GetRevisionId(IDictionary<string, TreeRevisionData> revisions, string platform)
        {
            var ids = revisions
                .Select(kvp => kvp.Value.Id)
                .Where(i => i != null);
            var id = ids.FirstOrDefault();
            if (id == null)
                throw new InvalidOperationException($"{platform}: Missing ID");
            if (ids.Any(i => !Equals(i.Id, id?.Id)))
                throw new InvalidOperationException($"{platform}: Mismatching IDs");
            return id;
        }

        private static TreeCardData GetCard(CameraData camera)
        {
            return camera != null
                ? camera.Card
                : new TreeCardData();
        }

        private TreeEncodingData GetEncoding(string platformPath, string sourcePlatform, string platform, IDictionary<string, TreeRevisionData> revisions)
        {
            return GetPlatformEncoding(platformPath, sourcePlatform, platform)
                ?? GetRevisionEncoding(revisions, platform)
                ?? GetEmptyEncoding();
        }

        private TreeEncodingData GetPlatformEncoding(string platformPath, string sourcePlatform, string platform)
        {
            return EncodingProvider.GetEncoding(platformPath, sourcePlatform ?? platform);
        }

        private TreeEncodingData GetRevisionEncoding(IDictionary<string, TreeRevisionData> revisions, string platform)
        {
            var encoding = revisions.First().Value.Encoding;
            if (revisions.Any(kvp => !Equals(kvp.Value.Encoding?.Version, encoding?.Version)))
                throw new InvalidOperationException($"{platform}: Mismatching encodings");
            return encoding;
        }

        private static TreeEncodingData GetEmptyEncoding()
        {
            return new TreeEncodingData
            {
                Version = 0
            };
        }
    }
}
