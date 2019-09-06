using Newtonsoft.Json;
using System.Collections.Generic;

namespace Net.Chdk.Model.Software
{
    [JsonObject(IsReference = false)]
    public sealed class SoftwareHashInfo
    {
        public string Name { get; set; }

        public IDictionary<string, string> Values { get; set; }
    }
}
