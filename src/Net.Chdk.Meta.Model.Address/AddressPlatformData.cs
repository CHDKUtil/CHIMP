using Net.Chdk.Json;
using Newtonsoft.Json;

namespace Net.Chdk.Meta.Model.Address
{
    public sealed class AddressPlatformData : PlatformData<AddressPlatformData, AddressRevisionData, PlatformSourceData>
    {
        [JsonConverter(typeof(HexStringJsonConverter))]
        public ushort Id { get; set; }
    }
}
