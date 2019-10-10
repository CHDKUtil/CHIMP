using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;

namespace Chimp.Model
{
    class DownloadData
    {
        public Uri BaseUri { get; set; }
        public string Path { get; set; }
        public string Date { get; set; }
        public string Size { get; set; }
        public string TargetPath { get; set; }
        public string RootDir { get; set; }
    }
    
    sealed class ScriptDownloadData : DownloadData
    {
        public ScriptDownloadData(IDictionary<string, object> substitutes, SoftwareInfo software, string path)
        {
            Substitutes = substitutes;
            Software = software;
            Path = path;
        }

        public IDictionary<string, object> Substitutes { get; }
        public SoftwareInfo Software { get; }
    }
}
