using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;

namespace Net.Chdk.Meta.Providers.Address.Src
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
                case "PLATFORMID":
                    value ??= new RevisionData();
                    value.Id = GetIdValue(split[1].Trim());
                    break;
                default:
                    break;
            }
        }

        private ushort GetIdValue(string version)
        {
            return ushort.Parse(version);
        }
    }
}
