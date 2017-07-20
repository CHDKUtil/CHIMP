using Chimp.Model;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers;
using Net.Chdk.Providers.Product;
using System.Collections.Generic;

namespace Chimp.Providers
{
    sealed class AggregateTipProvider : ProviderResolver<ITipProvider>, ITipProvider
    {
        private MainViewModel MainViewModel { get; }
        private DownloadViewModel DownloadViewModel => DownloadViewModel.Get(MainViewModel);
        private SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);

        private IServiceActivator ServiceActivator { get; }
        private IProductProvider ProductProvider { get; }

        public AggregateTipProvider(MainViewModel mainViewModel, IServiceActivator serviceActivator, IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainViewModel = mainViewModel;
            ServiceActivator = serviceActivator;
            ProductProvider = productProvider;
        }

        public IEnumerable<Tip> GetTips(string productText)
        {
            var productName =
                DownloadViewModel?.Software?.Product.Name
                ?? SoftwareViewModel?.SelectedItem?.Info.Product?.Name;
            ITipProvider provider = null;
            if (productName != null)
                provider = GetProvider(productName);
            if (provider == null)
                provider = new ProductTipProvider(ServiceActivator);
            return provider.GetTips(productText);
        }

        protected override ITipProvider CreateProvider(string name)
        {
            return new ProductTipProvider(ServiceActivator, name);
        }

        protected override IEnumerable<string> GetNames()
        {
            return ProductProvider.GetProductNames();
        }
    }
}
