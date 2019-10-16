using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software
{
    public sealed class MatchData : IMatchData<Match[]>
    {
        public MatchData(params Match[] payload)
        {
            Payload = payload;
        }

        public MatchData(string error)
        {
            Error = error;
        }

        public MatchData(IEnumerable<string>? platforms, IEnumerable<string>? revisions, IEnumerable<string>? builds)
        {
            Platforms = platforms;
            Revisions = revisions;
            Builds = builds;
        }

        public Match[]? Payload { get; }
        public string? Error { get; }
        public IEnumerable<string>? Platforms { get; }
        public IEnumerable<string>? Revisions { get; }
        public IEnumerable<string>? Builds { get; }
    }
}
