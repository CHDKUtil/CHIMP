using System.Collections.Generic;

namespace Chimp
{
    public interface IStepProvider
    {
        IEnumerable<string> GetSteps();
        bool IsSkip(string name);
        bool IsHidden(string name);
    }
}
