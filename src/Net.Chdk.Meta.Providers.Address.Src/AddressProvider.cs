using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;
using System.Globalization;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class AddressProvider : StubsDefParsingProvider<AddressData>
    {
        public AddressProvider(ILogger<AddressProvider> logger)
            : base(logger)
        {
        }

        public AddressData? GetAddresses(string platformPath, string platform, string revision)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string FileName => "stubs_min.S";

        protected override void UpdateValue(ref AddressData? value, string line, string platform)
        {
            var split = TrimParentheses(line, platform).Split(',');
            switch (split[0].Trim(' '))
            {
                case "palette_buffer_ptr":
                    value ??= new AddressData();
                    value.PaletteBufferPtr = GetAddress(split[1].Trim(' '));
                    break;
                case "active_palette_buffer":
                    value ??= new AddressData();
                    value.ActivePaletteBuffer = GetAddress(split[1].Trim(' '));
                    break;
                default:
                    break;
            }
        }

        private static uint GetAddress(string addressStr)
        {
            var split = addressStr.Split('+');
            var result = 0u;
            for (int i = 0; i < split.Length; i++)
            {
                var str = split[i].TrimStart("0x");
                result += uint.Parse(str, NumberStyles.HexNumber);
            }
            return result;
        }
    }
}
