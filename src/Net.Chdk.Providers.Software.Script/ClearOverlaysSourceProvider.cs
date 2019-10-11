using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software.Product;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ClearOverlaysSourceProvider : ProductSourceProvider
    {
        public ClearOverlaysSourceProvider(IProductProvider productProvider, ILogger<ClearOverlaysSourceProvider> logger)
            : base(productProvider, logger)
        {
        }

        protected override string ProductName => "clear_overlays";
    }
}
