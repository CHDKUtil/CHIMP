using Chimp.Model;
using System.Collections.Generic;

namespace Chimp
{
    interface ITipProvider
    {
        IEnumerable<Tip> GetTips(string productText);
    }
}
