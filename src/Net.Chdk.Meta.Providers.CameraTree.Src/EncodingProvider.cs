using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class EncodingProvider : MakefileParsingProvider<byte?>
    {
        public EncodingProvider(ILogger<EncodingProvider> logger)
            : base(logger)
        {
        }

        public byte? GetEncoding(string platformPath, string platform, string revision = null)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string Prefix => string.Empty;

        protected override void UpdateValue(ref byte? value, string line, string platform)
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

        private byte? GetEncodingValue(string version)
        {
            return byte.Parse(version);
        }
    }
}
