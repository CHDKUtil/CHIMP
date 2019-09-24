using System;
using System.Collections.Generic;
using System.Globalization;

namespace Chimp.Model
{
    sealed class Distro
    {
        public string? MatchType { get; set; }
        public string? ProductType { get; set; }
        public CultureInfo? Language { get; set; }
        public Uri? BaseUrl { get; set; }
        public Dictionary<string, string>? Builds { get; set; }
    }
}
