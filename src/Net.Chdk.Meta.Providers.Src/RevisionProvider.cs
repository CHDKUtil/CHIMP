using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class RevisionProvider<TRevision, TData>
        where TRevision : RevisionData<TRevision, PlatformSourceData>
        where TData : class
    {
        private SourceProvider<PlatformSourceData> SourceProvider { get; }
        private DataProvider<TData> DataProvider { get; }
        protected ILogger Logger { get; }

        protected RevisionProvider(SourceProvider<PlatformSourceData> sourceProvider, DataProvider<TData> dataProvider, ILogger logger)
        {
            SourceProvider = sourceProvider;
            DataProvider = dataProvider;
            Logger = logger;
        }

        public TRevision GetRevisionData(string platformPath, string platform, string revision)
        {
            Logger.LogTrace("Enter {0}", revision);

            var source = GetSource(platformPath, platform, revision);
            var sourcePlatform = GetPlatform(source, platform);
            var sourceRevision = GetRevision(source, revision);
            if (!sourcePlatform.Equals(platform) || !sourceRevision.Equals(revision))
                Logger.LogTrace("Source: {0}-{1}", sourcePlatform, sourceRevision);

            var data = GetData(platformPath, sourcePlatform, sourceRevision);
            var revisionData = GetRevisionData(platformPath, sourcePlatform, sourceRevision, data);
            revisionData.Source = source;

            Logger.LogTrace("Leave {0}", revision);
            return revisionData;
        }

        protected abstract TRevision GetRevisionData(string platformPath, string sourcePlatform, string sourceRevision, TData? data);

        private PlatformSourceData? GetSource(string platformPath, string platform, string revision)
        {
            return SourceProvider.GetSource(platformPath, platform, revision);
        }

        private string GetPlatform(PlatformSourceData? source, string platform)
        {
            return source?.Platform ?? platform;
        }

        private string GetRevision(PlatformSourceData? source, string revision)
        {
            return source?.Revision ?? revision;
        }

        private TData? GetData(string platformPath, string platform, string revision)
        {
            return DataProvider.GetData(platformPath, platform, revision);
        }
    }
}
