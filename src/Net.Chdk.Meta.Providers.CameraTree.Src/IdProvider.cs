using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class IdProvider : MakefileParsingProvider<ushort?>
    {
        public IdProvider(ILogger<IdProvider> logger)
            : base(logger)
        {
        }

        public ushort? GetId(string platformPath, string platform, string revision = null)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string Prefix => string.Empty;

        protected override void UpdateValue(ref ushort? value, string line, string platform)
        {
            var split = line.Split('=');
            switch (split[0].Trim())
            {
                case "PLATFORMID":
                    value = value ?? GetIdValue(split[1].Trim());
                    break;
                default:
                    break;
            }
        }

        private ushort? GetIdValue(string version)
        {
            return ushort.Parse(version);
        }
    }
}
