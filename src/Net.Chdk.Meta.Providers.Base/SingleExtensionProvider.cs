using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers
{
    public abstract class SingleExtensionProvider<TInnerProvider>
        where TInnerProvider : IExtensionProvider
    {
        private Dictionary<string, TInnerProvider> InnerProviders { get; }

        protected SingleExtensionProvider(IEnumerable<TInnerProvider> innerProviders)
        {
            InnerProviders = innerProviders.ToDictionary(
                p => p.Extension,
                p => p);
        }

        protected TInnerProvider GetInnerProvider(string path, out string ext)
        {
            ext = Path.GetExtension(path);
            InnerProviders.TryGetValue(ext, out TInnerProvider value);
            return value;
        }
    }
}
