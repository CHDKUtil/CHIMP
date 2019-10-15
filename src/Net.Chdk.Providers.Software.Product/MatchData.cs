using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software
{
    public abstract class MatchData<T> : IMatchData<T>
        where T : class
    {
        protected MatchData(T payload)
        {
            Payload = payload;
        }

        protected MatchData(string error)
        {
            Error = error;
        }

        protected MatchData(IEnumerable<string>? platforms, IEnumerable<string>? revisions, IEnumerable<string>? builds)
        {
            Platforms = platforms;
            Revisions = revisions;
            Builds = builds;
        }

        public T? Payload { get; }
        public bool Success => Payload != null;
        public string? Error { get; }
        public IEnumerable<string>? Platforms { get; }
        public IEnumerable<string>? Revisions { get; }
        public IEnumerable<string>? Builds { get; }
    }

    public sealed class MatchData : MatchData<Match[]>
    {
        public MatchData(params Match[] matches)
            : base(matches)
        {
        }

        public MatchData(string error)
            : base(error)
        {
        }

        public MatchData(ICollection<string> platforms, ICollection<string> revisions, ICollection<string> builds)
            : base(GetItems(platforms), GetItems(revisions), GetItems(builds))
        {
        }

        private static IEnumerable<string>? GetItems(ICollection<string> items)
        {
            return items?.Count > 0
                ? items.Distinct()
                : null;
        }
    }
}
