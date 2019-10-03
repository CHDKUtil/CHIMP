using Net.Chdk.Meta.Model.Platform;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Platform
{
    interface IInnerPlatformProvider
    {
        IEnumerable<KeyValuePair<string, PlatformData>>? GetPlatforms(uint modelId);
        PlatformData? GetPlatform(string platform);
    }
}
