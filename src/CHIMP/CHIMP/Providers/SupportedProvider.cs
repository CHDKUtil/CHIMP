using Chimp.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers
{
    sealed class SupportedProvider : ProviderResolver<ISupportedProvider>, ISupportedProvider
    {
        private IServiceActivator ServiceActivator { get; }
        private SupportedData Data { get; }

        public SupportedProvider(IOptions<SupportedData> options, IServiceActivator serviceActivator, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ServiceActivator = serviceActivator;
            Data = options.Value;
        }

        public string GetError(MatchData data)
        {
            return Providers.Values
                .Select(p => p.GetError(data))
                .FirstOrDefault(e => e != null);
        }

        public string[] GetItems(MatchData data, SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return Providers.Values
                .Select(p => p.GetItems(data, product, camera))
                .FirstOrDefault(i => i != null);
        }

        public string GetTitle(MatchData data)
        {
            return Providers.Values
                .Select(p => p.GetTitle(data))
                .FirstOrDefault(t => t != null);
        }

        protected override ISupportedProvider CreateProvider(string name)
        {
            var @namespace = "Chimp.Providers.Supported";
            var type = $"Supported{name}Provider";
            return ServiceActivator.Create<ISupportedProvider>($"{@namespace}.{type}", null, null);
        }

        protected override IEnumerable<string> GetNames()
        {
            return Data.Supported;
        }
    }
}
