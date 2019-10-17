using Net.Chdk.Json;
using Newtonsoft.Json;

namespace Net.Chdk.Model.Software
{
    [JsonObject(IsReference = false)]
    public sealed class SoftwareModelInfo
    {
        [JsonConverter(typeof(HexStringJsonConverter))]
        public uint Id { get; set; }

        public string[]? Names { get; set; }
    }
}
