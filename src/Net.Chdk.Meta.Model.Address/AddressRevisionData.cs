using Net.Chdk.Json;
using Newtonsoft.Json;

namespace Net.Chdk.Meta.Model.Address
{
    public sealed class AddressRevisionData : RevisionData<AddressRevisionData, PlatformSourceData>
    {
        [JsonConverter(typeof(HexStringJsonConverter))]
        public ushort? Id { get; set; }

        [JsonProperty("revision_str_address")]
        [JsonConverter(typeof(HexStringJsonConverter))]
        public uint RevisionAddress { get; set; }

        [JsonProperty("palette_buffer_ptr")]
        [JsonConverter(typeof(HexStringJsonConverter))]
        public uint? PaletteBufferPtr { get; set; }

        [JsonProperty("active_palette_buffer")]
        [JsonConverter(typeof(HexStringJsonConverter))]
        public uint? ActivePaletteBuffer { get; set; }

        [JsonProperty("palette_to_zero")]
        public uint PaletteToZero { get; set; }
    }
}
