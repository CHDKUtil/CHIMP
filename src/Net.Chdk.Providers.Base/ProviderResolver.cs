using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers
{
    public abstract class ProviderResolver<TProvider>
        where TProvider : class
    {
        #region Fields

        protected ILoggerFactory LoggerFactory { get; }

        #endregion

        #region Constructor

        protected ProviderResolver(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;

            providers = new Lazy<Dictionary<string, TProvider>>(GetProviders);
        }

        #endregion

        #region Providers

        protected TProvider? GetProvider(string? name)
        {
            if (name == null)
                return null;
            Providers.TryGetValue(name, out TProvider? provider);
            return provider;
        }

        private readonly Lazy<Dictionary<string, TProvider>> providers;

        protected Dictionary<string, TProvider> Providers => providers.Value;

        private Dictionary<string, TProvider> GetProviders()
        {
            return GetNames()
                .Select(CreateKeyValuePair)
                .Where(kvp => kvp.Value != null)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!);
        }

        private KeyValuePair<string, TProvider?> CreateKeyValuePair(string name)
        {
            var provider = CreateProvider(name);
            return new KeyValuePair<string, TProvider?>(name, provider);
        }

        protected abstract IEnumerable<string> GetNames();

        protected abstract TProvider? CreateProvider(string name);

        #endregion
    }
}
