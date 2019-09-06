using Net.Chdk.Json;
using Newtonsoft.Json;

namespace Net.Chdk.Model.Software
{
    public sealed class SoftwareEncodingInfo
    {
        public static readonly SoftwareEncodingInfo Empty = new SoftwareEncodingInfo
        {
            Name = string.Empty
        };

        public string Name { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter), "x8")]
        public uint? Data { get; set; }
    }
}
