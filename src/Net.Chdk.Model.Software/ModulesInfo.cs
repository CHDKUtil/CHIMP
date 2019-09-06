using System;
using System.Collections.Generic;

namespace Net.Chdk.Model.Software
{
    public sealed class ModulesInfo
    {
        public Version Version { get; set; }
        public ModulesProductInfo Product { get; set; }
        public IDictionary<string, ModuleInfo> Modules { get; set; }
    }
}
