using System;
using System.Threading.Tasks;

namespace Chimp
{
    public interface IControllerContainer : IDisposable
    {
        Task<IController> GetControllerAsync(string name);
    }
}
