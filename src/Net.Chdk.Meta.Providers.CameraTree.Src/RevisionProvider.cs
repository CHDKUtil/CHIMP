using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class RevisionProvider
    {
        private ILogger Logger { get; }
        private SourceProvider SourceProvider { get; }
        private DataProvider DataProvider { get; }

        public RevisionProvider(SourceProvider sourceProvider, DataProvider dataProvider, ILogger<RevisionProvider> logger)
        {
            SourceProvider = sourceProvider;
            DataProvider = dataProvider;
            Logger = logger;
        }

        public TreeRevisionData GetRevisionData(string platformPath, string platform, string revision)
        {
            Logger.LogTrace("Enter {0}", revision);

            var source = GetSource(platformPath, platform, revision);
            var sourcePlatform = GetPlatform(source, platform);
            var sourceRevision = GetRevision(source, revision);
            if (!sourcePlatform.Equals(platform) || !sourceRevision.Equals(revision))
                Logger.LogTrace("Source: {0}-{1}", sourcePlatform, sourceRevision);

            var data = GetData(platformPath, sourcePlatform, sourceRevision);
            var id = data?.Id;
            if (id != null)
                Logger.LogTrace("ID: {0}", id);
            var encoding = data?.Encoding;
            if (encoding != null)
                Logger.LogTrace("Encoding: {0}", encoding);

            var revisionData = new TreeRevisionData
            {
                Source = source,
                Id = id,
                Encoding = encoding,
            };

            Logger.LogTrace("Leave {0}", revision);
            return revisionData;
        }

        private TreeSourceData GetSource(string platformPath, string platform, string revision)
        {
            return SourceProvider.GetSource(platformPath, platform, revision);
        }

        private RevisionData GetData(string platformPath, string platform, string revision)
        {
            return DataProvider.GetData(platformPath, platform, revision);
        }

        private string GetPlatform(TreeSourceData source, string platform)
        {
            return source != null
                ? source.Platform ?? platform
                : platform;
        }

        private string GetRevision(TreeSourceData source, string revision)
        {
            return source != null
                ? source.Revision ?? revision
                : revision;
        }
    }
}
