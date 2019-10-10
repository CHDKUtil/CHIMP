using System.Threading;
using System.Threading.Tasks;
using Chimp.Model;

namespace Chimp
{
    interface IAction
    {
        string DisplayName { get; }
        bool IsDefault { get; }

        Task<SoftwareData> PerformAsync(CancellationToken cancellationToken);
    }
}