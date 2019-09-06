using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software.Product;

namespace Net.Chdk.Providers.Software.Sdm
{
    sealed class SdmSourceProvider : ProductSourceProvider
    {
        public SdmSourceProvider(IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(productProvider, loggerFactory.CreateLogger<SdmSourceProvider>())
        {
        }

        protected override string ProductName => "SDM";
    }
}
