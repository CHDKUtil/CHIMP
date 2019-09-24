using Net.Chdk.Model.Software;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp
{
    interface IMatchProvider
    {
        Task<Match[]?> GetMatchesAsync(SoftwareCameraInfo camera, string buildName, CancellationToken cancellationToken);
        string GetError();
    }
}
