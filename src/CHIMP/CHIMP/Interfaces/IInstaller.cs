using System.Threading;
using System.Threading.Tasks;

namespace Chimp
{
    interface IInstaller
    {
        Task<bool> InstallAsync(CancellationToken cancellationToken);
    }
}
