using Net.Chdk.Generators.Script;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chimp.Model
{
    class MatchData
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

        public Match[] Matches { get; protected set; }
        public string Error { get; protected set; }
        public IEnumerable<string> Platforms { get; protected set; }
        public IEnumerable<string> Revisions { get; protected set; }
        public IEnumerable<string> Builds { get; protected set; }
        public SoftwareInfo Software { get; set; }
    }

    sealed class ScriptMatchData : MatchData
    {
        public ScriptMatchData(IDictionary<string, object> substitutes)
        {
            Substitutes = substitutes;

            // Matches is empty by default
            if (!Substitutes.ContainsKey("revision"))
                Matches = null;

            if (Substitutes.TryGetValue("error", out string error))
                Error = error;
            if (Substitutes.TryGetValue("platforms", out IEnumerable<string> platforms))
                Platforms = platforms;
            if (Substitutes.TryGetValue("revisions", out IEnumerable<string> revisions))
                Revisions = revisions;
            if (Substitutes.TryGetValue("builds", out IEnumerable<string> builds))
                Builds = builds;
        }

        public IDictionary<string, object> Substitutes { get; }
    }
}
