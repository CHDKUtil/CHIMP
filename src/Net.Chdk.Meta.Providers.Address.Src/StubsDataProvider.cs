using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;
using Net.Chdk.Providers.Firmware;
using System;
using System.Globalization;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class StubsDataProvider : ParsingProvider<StubsData>
    {
        private const string CategoryName = "PS";

        private IFirmwareProvider FirmwareProvider { get; }

        public StubsDataProvider(IFirmwareProvider firmwareProvider, ILogger<StubsDataProvider> logger)
            : base(logger)
        {
            FirmwareProvider = firmwareProvider;
        }

        public StubsData GetData(string platformPath, string platform, string? revision)
        {
            var value = GetValue(platformPath, platform, revision);
            if (value == null)
                throw new InvalidOperationException($"{platform}-{revision}: Missing revision data");
            return value;
        }

        protected override void UpdateValue(ref StubsData? value, string line, string platform, string? revision)
        {
            var split = line.Split(' ');
            switch (split[0])
            {
                case "PLATFORMID":
                    UpdatePlatformId(ref value, split, platform, revision);
                    break;
                case "Firmware" when split[1].StartsWith("Ver"):
                    UpdateRevision(ref value, split, platform, revision);
                    break;
                default:
                    break;
            }
        }

        private static void UpdatePlatformId(ref StubsData? value, string[] split, string platform, string? revision)
        {
            if (value?.IdAddress != null)
                throw new InvalidOperationException($"{platform}-{revision}: Duplicate platform ID address");

            if (revision == null)
                throw new ArgumentNullException(nameof(revision));

            value ??= new StubsData();
            value.Id = ushort.TryParse(split[2], out var idValue)
                ? idValue
                : (ushort?)null;
            value.IdAddress = GetAddress(split, platform, revision);
        }

        private void UpdateRevision(ref StubsData? value, string[] split, string platform, string? revision)
        {
            if (value?.RevisionAddress != null)
                throw new InvalidOperationException($"{platform}-{revision}: Duplicate revision address");

            if (revision == null)
                throw new ArgumentNullException(nameof(revision));

            var revisionStr = split[2];
            var revisionAddress = GetAddress(split, platform, revision);

            if (revisionStr.StartsWith("GM"))
            {
                revisionStr = revisionStr.Substring(2);
                revisionAddress += 2;
            }

            if (revisionStr != GetRevisionString(revision))
                throw new InvalidOperationException($"{platform}-{revision}: Invalid revision string");

            value ??= new StubsData();
            value.RevisionAddress = revisionAddress;
        }

        protected override string TrimComments(string line, string platform, string? revision)
        {
            return line;
        }

        protected override string Prefix => "//   ";

        protected override string FileName => "stubs_entry.S";

        private static uint GetAddress(string[] split, string platform, string revision)
        {
            if (split[split.Length - 2] != "@")
                throw new InvalidOperationException($"{platform}-{revision}: Invalid address string");
            return uint.Parse(split[split.Length - 1].Substring(2), NumberStyles.HexNumber);
        }

        private string? GetRevisionString(string revision)
        {
            return FirmwareProvider.GetRevisionString(revision, CategoryName);
        }
    }
}
