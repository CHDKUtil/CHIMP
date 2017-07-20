using System.Threading;
using System.Threading.Tasks;

namespace Chimp
{
    interface IToastService
    {
		bool IsAvailable { get; }
		Task<bool> ShowEjectToastAsync(string displayName);
    }
}
