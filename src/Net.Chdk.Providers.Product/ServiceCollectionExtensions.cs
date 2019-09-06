using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Product
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductProvider, ProductProvider>();
        }
    }
}
