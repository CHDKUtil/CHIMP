using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers
{
    public abstract class SingleProductProvider<TInnerProvider>
        where TInnerProvider : IProductNameProvider
    {
        private Dictionary<string, TInnerProvider> InnerProviders { get; }

        protected SingleProductProvider(IEnumerable<TInnerProvider> innerProviders)
        {
            InnerProviders = innerProviders.ToDictionary(
                p => p.ProductName,
                p => p);
        }

        protected TInnerProvider GetInnerProvider(string productName)
        {
            if (!InnerProviders.TryGetValue(productName, out TInnerProvider value))
                throw new InvalidOperationException($"Unknown product: {productName}");
            return value;
        }
    }
}
