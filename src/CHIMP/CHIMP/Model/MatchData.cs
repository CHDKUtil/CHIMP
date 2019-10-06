using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chimp.Model
{
    sealed class MatchData
    {
        public MatchData(params Match[] matches)
        {
            Matches = matches;
        }

        public MatchData(string error)
        {
            Error = error;
        }

        public MatchData(IEnumerable<string> platforms, IEnumerable<string> revisions, IEnumerable<string> builds)
        {
            Platforms = platforms?.Count() > 0
                ? platforms.Distinct()
                : null;
            Revisions = revisions?.Count() > 0
                ? revisions.Distinct()
                : null;
            Builds = builds?.Count() > 0
                ? builds.Distinct()
                : null;
        }

        public Match[] Matches { get; }
        public string Error { get; }
        public IEnumerable<string> Platforms { get; }
        public IEnumerable<string> Revisions { get; }
        public IEnumerable<string> Builds { get; }
    }
}
