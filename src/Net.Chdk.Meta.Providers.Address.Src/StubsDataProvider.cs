using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;
using System;
using System.Globalization;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class StubsDataProvider : ParsingProvider<RevisionData>
    {
        public StubsDataProvider(ILogger<StubsDataProvider> logger)
            : base(logger)
        {
        }

        public RevisionData? GetData(string platformPath, string platform, string? revision)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override void UpdateValue(ref RevisionData? value, string line, string platform, string? revision)
        {
            if (value != null)
                throw new InvalidOperationException($"{platform}-{revision}: Duplicate platform ID address");

            var split = line.Split(' ');
            var id = ushort.TryParse(split[0], out var idValue)
                ? idValue
                : (ushort?)null;
            var idAddress = uint.Parse(split[split.Length - 1].Substring(2), NumberStyles.HexNumber);

            value = new RevisionData
            {
                Id = id,
                Address = idAddress
            };
        }

        protected override string TrimComments(string line, string platform, string? revision)
        {
            return line;
        }

        protected override string Prefix => "//   PLATFORMID = ";

        protected override string FileName => "stubs_entry.S";
    }
}
