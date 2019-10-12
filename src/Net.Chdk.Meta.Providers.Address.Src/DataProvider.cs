using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;
using System.Globalization;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class DataProvider : DataProvider<RevisionData>
    {
        public DataProvider(ILogger<DataProvider> logger)
            : base(logger)
        {
        }

        protected override void UpdateValue(ref RevisionData? value, string line, string platform, string? revision)
        {
            var address = GetAddress(line);
            if (address != null)
            {
                value ??= new RevisionData();
                value.IdAddress = address;
            }

            var split = base.TrimComments(line, platform, revision).Split('=');
            switch (split[0].Trim())
            {
                case "THUMB_FW":
                    value ??= new RevisionData();
                    value.Thumb = GetBoolean(split, platform, revision);
                    break;
                case "PLATFORMID":
                    value ??= new RevisionData();
                    value.Id = GetIdValue(split[1].Trim());
                    break;
                default:
                    break;
            }
        }

        protected override string TrimComments(string line, string platform, string? revision)
        {
            return line;
        }

        private static uint? GetAddress(string line)
        {
            if (!line.Contains("PLATFORMID") || !line.Contains("@"))
                return null;
            var split = line.Split(' ');
            var index = System.Array.IndexOf(split, "@");
            return uint.Parse(split[index + 1].Substring(2), NumberStyles.HexNumber);
        }

        private static ushort GetIdValue(string version)
        {
            return ushort.Parse(version);
        }
    }
}
