using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class EncodingProvider : MakefileParsingProvider<TreeEncodingData>
    {
        public EncodingProvider(ILogger<EncodingProvider> logger)
            : base(logger)
        {
        }

        public TreeEncodingData GetEncoding(string platformPath, string platform, string revision = null)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string Prefix => string.Empty;

        protected override void UpdateValue(ref TreeEncodingData value, string line, string platform)
        {
            var split = line.Split('=');
            switch (split[0].Trim())
            {
                case "NEED_ENCODED_DISKBOOT":
                    value = value ?? GetEncodingValue(split[1].Trim());
                    break;
                default:
                    break;
            }
        }

        private TreeEncodingData GetEncodingValue(string version)
        {
            return new TreeEncodingData
            {
                Version = uint.Parse(version)
            };
        }
    }
}
