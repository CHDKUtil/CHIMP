using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Net.Chdk.Model.Software
{
    [JsonObject(IsReference = false)]
    public sealed class SoftwareProductInfo
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public string VersionPrefix { get; set; }
        public string VersionSuffix { get; set; }
        public DateTime? Created { get; set; }
        public CultureInfo Language { get; set; }
    }
}
