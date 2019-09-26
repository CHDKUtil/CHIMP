using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Providers.Src;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class RevisionProvider : RevisionProvider<TreeRevisionData, RevisionData>
    {
        public RevisionProvider(SourceProvider sourceProvider, DataProvider dataProvider, ILogger<RevisionProvider> logger)
            : base(sourceProvider, dataProvider, logger)
        {
        }

        protected override TreeRevisionData GetRevisionData(string platformPath, string platform, string revision, RevisionData? data)
        {
            var encoding = data?.Encoding;
            if (encoding != null)
                Logger.LogTrace("Encoding: {0}", encoding);

            return new TreeRevisionData
            {
                Encoding = encoding,
            };
        }
    }
}
