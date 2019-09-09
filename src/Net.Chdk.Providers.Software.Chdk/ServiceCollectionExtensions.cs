using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Software.Chdk
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChdkSourceProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductSourceProvider, ChdkSourceProvider>();
        }
    }
}
