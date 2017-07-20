using Chimp.ViewModels;
using System;
using System.Threading.Tasks;

namespace Chimp
{
    public interface IController : IDisposable
    {
        Task InitializeAsync();
        Task EnterStepAsync();
        Task LeaveStepAsync();
    }
}
