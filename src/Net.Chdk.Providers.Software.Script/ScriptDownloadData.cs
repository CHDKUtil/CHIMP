using Net.Chdk.Model.Software;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptDownloadData : IDownloadData
    {
        public ScriptDownloadData(IDictionary<string, object> substitutes, SoftwareInfo software, string path)
        {
            Substitutes = substitutes;
            Software = software;
            Path = path;
        }

        public IDictionary<string, object> Substitutes { get; }
        public SoftwareInfo Software { get; }
        public string Path { get; }

        public string? TargetPath => null;
        public string? RootDir => null;
    }
}
