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

        public MatchData(string error, ICollection<string> platforms = null, ICollection<string> revisions = null, ICollection<string> builds = null)
        {
            Error = error;
            if (platforms?.Count > 0)
                Platforms = platforms.Distinct();
            if (revisions?.Count > 0)
                Revisions = revisions.Distinct();
            if (builds?.Count > 0)
                Builds = builds.Distinct();
        }

        public Match[] Matches { get; }
        public string Error { get; }
        public IEnumerable<string> Platforms { get; }
        public IEnumerable<string> Revisions { get; }
        public IEnumerable<string> Builds { get; }
    }
}
