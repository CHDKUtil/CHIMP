using System.Collections.Generic;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptMatchData : IMatchData<IDictionary<string, object>>
    {
        public ScriptMatchData(IDictionary<string, object> substitutes)
        {
            Payload = substitutes;
        }

        public ScriptMatchData(string error)
        {
            Error = error;
        }

        public ScriptMatchData(IEnumerable<string>? platforms = null, IEnumerable<string>? revisions = null, IEnumerable<string>? builds = null)
        {
            Platforms = platforms;
            Revisions = revisions;
            Builds = builds;
        }

        public IDictionary<string, object>? Payload { get; }
        public string? Error { get; }
        public IEnumerable<string>? Platforms { get; }
        public IEnumerable<string>? Revisions { get; }
        public IEnumerable<string>? Builds { get; }
    }
}
