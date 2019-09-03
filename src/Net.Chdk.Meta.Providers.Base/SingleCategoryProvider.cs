using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers
{
    public abstract class SingleCategoryProvider<TInnerProvider>
        where TInnerProvider : ICategoryNameProvider
    {
        private Dictionary<string, TInnerProvider> InnerProviders { get; }

        protected SingleCategoryProvider(IEnumerable<TInnerProvider> innerProviders)
        {
            InnerProviders = innerProviders.ToDictionary(
                p => p.CategoryName,
                p => p);
        }

        protected TInnerProvider GetInnerProvider(string categoryName)
        {
            if (!InnerProviders.TryGetValue(categoryName, out TInnerProvider value))
                throw new InvalidOperationException($"Unknown category: {categoryName}");
            return value;
        }
    }
}
