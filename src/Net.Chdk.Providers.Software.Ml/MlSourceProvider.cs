using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software.Product;

namespace Net.Chdk.Providers.Software.Ml
{
    sealed class MlSourceProvider : ProductSourceProvider
    {
        public MlSourceProvider(IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(productProvider, loggerFactory.CreateLogger<MlSourceProvider>())
        {
        }

        protected override string ProductName => "ML";
    }
}
