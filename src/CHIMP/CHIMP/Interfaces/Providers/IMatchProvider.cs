using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp
{
    interface IMatchProvider
    {
        Task<IMatchData> GetMatchesAsync(SoftwareInfo software, string buildName, CancellationToken cancellationToken);
    }
}
