using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;
using System;
using System.Globalization;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class RevisionAddressData
    {
        public uint Address { get; set; }
    }

    sealed class RevisionAddressProvider : ParsingProvider<RevisionAddressData>
    {
        public RevisionAddressProvider(ILogger<RevisionAddressProvider> logger)
            : base(logger)
        {
        }

        public uint GetRevisionAddress(string platformPath, string platform, string? revision)
        {
            var value = GetValue(platformPath, platform, revision);
            if (value == null)
                throw new InvalidOperationException($"{platform}-{revision}: Missing revision address");
            return value.Address;
        }

        protected override void UpdateValue(ref RevisionAddressData? value, string line, string platform, string? revision)
        {
            if (value != null)
                throw new InvalidOperationException($"{platform}-{revision}: Duplicate revision address");

            var split = line.TrimStart("sion").TrimStart().Split(' ');

            var revisionStr = split[0];
            var revisionAddress = uint.Parse(split[split.Length - 1].Substring(2), NumberStyles.HexNumber);

            if (revisionStr.StartsWith("GM"))
            {
                revisionStr = revisionStr.Substring(2);
                revisionAddress += 2;
            }

            if (revisionStr.Length != 5 || revisionStr[1] != '.')
                throw new InvalidOperationException($"{platform}-{revision}: Invalid revision string");

            value = new RevisionAddressData
            {
                Address = revisionAddress
            };
        }

        protected override void UpdateValue(ref RevisionAddressData? value, string line, string platform)
        {
            throw new NotImplementedException();
        }

        protected override string TrimComments(string line, string platform, string? revision)
        {
            return line;
        }

        protected override string Prefix => "//   Firmware Ver";

        protected override string FileName => "stubs_entry.S";
    }
}
