using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class DataProvider : DataProvider<RevisionData>
    {
        public DataProvider(ILogger<DataProvider> logger)
            : base(logger)
        {
        }

        protected override void UpdateValue(ref RevisionData? value, string line, string platform)
        {
            var split = line.Split('=');
            switch (split[0].Trim())
            {
                case "NEED_ENCODED_DISKBOOT":
                    value ??= new RevisionData();
                    value.Encoding = GetEncodingValue(split[1].Trim());
                    break;
                default:
                    break;
            }
        }

        private byte GetEncodingValue(string version)
        {
            return byte.Parse(version);
        }
    }
}
