using Net.Chdk.Model.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Net.Chdk.Providers.Software
{
    public interface IMatchProvider
    {
    }

    public interface IMatchProvider<TMatchData> : IMatchProvider
        where TMatchData : IMatchData
    {
        Task<TMatchData> GetMatchesAsync(SoftwareInfo software, string buildName, CancellationToken cancellationToken);
    }
}
