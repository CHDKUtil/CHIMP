using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    sealed class InstallActionProvider : InstallActionProvider<InstallAction>
    {
        private IProductProvider ProductProvider { get; }

        public InstallActionProvider(MainViewModel mainViewModel, IProductProvider productProvider, ISourceProvider sourceProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, sourceProvider, serviceActivator)
        {
            ProductProvider = productProvider;
        }

        protected override IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            var infoProduct = SoftwareViewModel.SelectedItem?.Info.Product;
            var infoSources = infoProduct == null
                ? Enumerable.Empty<ProductSource>()
                : base.GetSources(infoProduct);
            return base.GetSources(product)
                .Except(infoSources);
        }

        protected override IEnumerable<SoftwareProductInfo> GetProducts()
        {
            var product = SoftwareViewModel.SelectedItem?.Info.Product;
            yield return new SoftwareProductInfo
            {
                Name = product?.Name,
            };
        }
    }
}
