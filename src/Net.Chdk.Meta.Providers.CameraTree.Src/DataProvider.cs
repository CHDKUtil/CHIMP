using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class DataProvider : MakefileParsingProvider<RevisionData>
    {
        public DataProvider(ILogger<DataProvider> logger)
            : base(logger)
        {
        }

        public RevisionData GetData(string platformPath, string platform, string revision = null)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string Prefix => string.Empty;

        protected override void UpdateValue(ref RevisionData value, string line, string platform)
        {
            var split = line.Split('=');
            switch (split[0].Trim())
            {
                case "PLATFORMID":
                    value = value ?? new RevisionData();
                    value.Id = GetIdValue(split[1].Trim());
                    break;
                case "NEED_ENCODED_DISKBOOT":
                    value = value ?? new RevisionData();
                    value.Encoding = GetEncodingValue(split[1].Trim());
                    break;
                default:
                    break;
            }
        }

        private ushort GetIdValue(string version)
        {
            return ushort.Parse(version);
        }

        private byte GetEncodingValue(string version)
        {
            return byte.Parse(version);
        }
    }
}
