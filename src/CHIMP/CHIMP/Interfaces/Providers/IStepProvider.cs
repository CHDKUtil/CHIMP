using System.Collections.Generic;

namespace Chimp
{
    public interface IStepProvider
    {
        IEnumerable<string> GetSteps();
    }
}
