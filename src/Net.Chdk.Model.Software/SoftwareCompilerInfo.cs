using System;

namespace Net.Chdk.Model.Software
{
    public sealed class SoftwareCompilerInfo
    {
        public string Name { get; set; }
        public string Platform { get; set; }
        public Version Version { get; set; }
    }
}