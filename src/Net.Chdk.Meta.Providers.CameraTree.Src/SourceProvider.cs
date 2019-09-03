using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class SourceProvider : MakefileParsingProvider<TreeSourceData>
    {
        public SourceProvider(ILogger<SourceProvider> logger)
            : base(logger)
        {
        }

        public TreeSourceData GetSource(string platformPath, string platform, string revision = null)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string Prefix => "override";

        protected override void UpdateValue(ref TreeSourceData value, string line, string platform)
        {
            var split = line.Split('=');
            switch (split[0])
            {
                case "PLATFORM":
                    value = value ?? new TreeSourceData();
                    value.Platform = split[1];
                    break;
                case "PLATFORMSUB":
                    value = value ?? new TreeSourceData();
                    value.Revision = split[1];
                    break;
                default:
                    break;
            }
        }
    }
}
