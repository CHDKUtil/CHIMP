using Net.Chdk.Model.Category;
using Newtonsoft.Json;
using System;

namespace Net.Chdk.Model.Software
{
    [JsonObject(IsReference = false)]
    public sealed class SoftwareInfo
    {
        public Version Version { get; set; }
        public CategoryInfo Category { get; set; }
        public SoftwareProductInfo Product { get; set; }
        public SoftwareCameraInfo Camera { get; set; }
        public SoftwareBuildInfo Build { get; set; }
        public SoftwareCompilerInfo Compiler { get; set; }
        public SoftwareSourceInfo Source { get; set; }
        public SoftwareEncodingInfo Encoding { get; set; }
        public SoftwareHashInfo Hash { get; set; }
    }
}
