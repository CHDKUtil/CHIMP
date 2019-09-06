using System;

namespace Net.Chdk.Model.Software
{
    public sealed class ModuleInfo
    {
        public DateTime? Created { get; set; }
        public string Changeset { get; set; }
        public SoftwareHashInfo Hash { get; set; }
    }
}