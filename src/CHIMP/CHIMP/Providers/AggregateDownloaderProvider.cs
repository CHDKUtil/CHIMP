using Chimp.Resolvers;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers
{
    sealed class AggregateDownloaderProvider : IDownloaderProvider
    {
        private IServiceActivator ServiceActivator { get; }
        private IProductProvider ProductProvider { get; }

        public AggregateDownloaderProvider(IServiceActivator serviceActivator, IProductProvider productProvider)
        {
            ServiceActivator = serviceActivator;
            ProductProvider = productProvider;
            _downloaderProviders = new Lazy<IEnumerable<IDownloaderProvider>>(GetDownloaderProviders);
        }

        private readonly Lazy<IEnumerable<IDownloaderProvider>> _downloaderProviders;

        private IEnumerable<IDownloaderProvider> DownloaderProviders => _downloaderProviders.Value;

        private IEnumerable<IDownloaderProvider> GetDownloaderProviders()
        {
            return ProductProvider.GetProductNames()
                .Select(CreateInnerProvider);
        }

        public IDownloader? GetDownloader(string productName, string sourceName, SoftwareSourceInfo source)
        {
            return DownloaderProviders
                .Select(p => p.GetDownloader(productName, sourceName, source))
                .FirstOrDefault(d => d != null);
        }

        private IDownloaderProvider CreateInnerProvider(string productName)
        {
            return new DownloaderResolver(productName, ServiceActivator);
        }
    }
}
