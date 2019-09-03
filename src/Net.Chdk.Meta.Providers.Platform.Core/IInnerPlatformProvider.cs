using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.Platform
{
    public interface IInnerPlatformProvider : IExtensionProvider
    {
        IEnumerable<KeyValuePair<string, string>> GetPlatforms(TextReader reader);
    }
}
