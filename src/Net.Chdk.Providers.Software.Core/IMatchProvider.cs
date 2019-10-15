using Net.Chdk.Model.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Net.Chdk.Providers.Software
{
    public interface IMatchProvider
    {
        Task<IMatchData> GetMatchesAsync(SoftwareInfo software, string buildName, CancellationToken cancellationToken);
    }
}
