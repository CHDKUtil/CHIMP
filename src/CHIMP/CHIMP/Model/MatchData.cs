using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chimp.Model
{
    class MatchData : IMatchData
    {
        public MatchData(params Match[] matches)
        {
            Matches = matches;
        }

        public MatchData(string error)
        {
            Error = error;
        }

        public MatchData(ICollection<string> platforms, ICollection<string> revisions, ICollection<string> builds)
        {
            Platforms = platforms?.Count > 0
                ? platforms.Distinct()
                : null;
            Revisions = revisions?.Count > 0
                ? revisions.Distinct()
                : null;
            Builds = builds?.Count > 0
                ? builds.Distinct()
                : null;
        }

        public bool Success => Matches != null;
        public Match[] Matches { get; protected set; }
        public string Error { get; protected set; }
        public IEnumerable<string> Platforms { get; protected set; }
        public IEnumerable<string> Revisions { get; protected set; }
        public IEnumerable<string> Builds { get; protected set; }
    }

    sealed class ScriptMatchData : MatchData
    {
        public ScriptMatchData(IDictionary<string, object> substitutes)
        {
            Substitutes = substitutes;
        }

        public ScriptMatchData(IEnumerable<string> platforms = null, IEnumerable<string> revisions = null)
        {
            // Matches is empty by default
            Matches = null;

            Platforms = platforms;
            Revisions = revisions;
        }

        public IDictionary<string, object> Substitutes { get; }
    }
}
