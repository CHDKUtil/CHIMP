using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    public interface IMatchData
    {
        bool Success { get; }
        string? Error { get; }
        IEnumerable<string>? Platforms { get; }
        IEnumerable<string>? Revisions { get; }
        IEnumerable<string>? Builds { get; }
    }
}
