using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    public interface IMatchData
    {
        string? Error { get; }
        IEnumerable<string>? Platforms { get; }
        IEnumerable<string>? Revisions { get; }
        IEnumerable<string>? Builds { get; }
    }

    public interface IMatchData<T> : IMatchData
        where T : class
    {
        T? Payload { get; }
    }
}
