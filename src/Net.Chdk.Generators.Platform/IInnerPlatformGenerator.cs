using System.Collections.Generic;

namespace Net.Chdk.Generators.Platform
{
    interface IInnerPlatformGenerator
    {
        string? GetPlatform(uint modelId, IEnumerable<string[]> splits, string? category);
    }
}
