using Chimp.Model;
using Net.Chdk.Model.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp
{
    interface IMatchProvider
    {
        Task<MatchData> GetMatchesAsync(SoftwareCameraInfo camera, string buildName, CancellationToken cancellationToken);
    }
}
