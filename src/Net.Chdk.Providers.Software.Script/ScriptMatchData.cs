using System.Collections.Generic;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptMatchData : MatchData<IDictionary<string, object>>
    {
        public ScriptMatchData(IDictionary<string, object> substitutes)
            : base(substitutes)
        {
        }

        public ScriptMatchData(string error)
            : base(error)
        {
        }

        public ScriptMatchData(IEnumerable<string>? platforms = null, IEnumerable<string>? revisions = null, IEnumerable<string>? builds = null)
            : base(platforms, revisions, builds)
        {
        }
    }
}
