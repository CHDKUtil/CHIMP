using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;
using System.Collections.Generic;
using System.Globalization;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class IdAddressProvider : HeaderParsingProvider<Dictionary<ushort, uint>>
    {
        public IdAddressProvider(ILogger<IdAddressProvider> logger)
            : base(logger)
        {
        }

        public Dictionary<ushort,uint>? GetData(string loaderPath)
        {
            return GetValue(loaderPath, "generic", null);
        }

        protected override string FileName => "compat_table.h";

        protected override string Prefix => "{ ";

        protected override void UpdateValue(ref Dictionary<ushort, uint>? value, string line, string? platform)
        {
            var split = line.TrimEnd(',').TrimEnd('}').Split(',');
            if (split.Length == 4)
            {
                var pid = ushort.Parse(split[0].Trim());
                var pid_addr = uint.Parse(split[3].Trim().Substring(2), NumberStyles.HexNumber);
                (value ??= new Dictionary<ushort, uint>()).Add(pid, pid_addr);
            }
        }
    }
}
