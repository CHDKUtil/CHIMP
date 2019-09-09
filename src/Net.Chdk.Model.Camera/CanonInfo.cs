using Net.Chdk.Json;
using Newtonsoft.Json;
using System;

namespace Net.Chdk.Model.Camera
{
    public sealed class CanonInfo
    {
        [JsonConverter(typeof(HexStringJsonConverter))]
        public uint ModelId { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        public uint FirmwareRevision { get; set; }

        public Version FirmwareVersion { get; set; }
    }
}
