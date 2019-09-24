using System;

namespace Chimp.Model
{
    public sealed class DownloadData
    {
        public Uri? BaseUri { get; set; }
        public string? Path { get; set; }
        public string? Date { get; set; }
        public string? Size { get; set; }
        public string? TargetPath { get; set; }
        public string? RootDir { get; set; }
    }
}
