using System.Collections.Generic;

namespace Chimp
{
    interface IActionProvider
    {
        IEnumerable<IAction> GetActions();
    }
}
