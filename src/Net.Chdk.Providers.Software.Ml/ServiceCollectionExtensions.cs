using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Software.Ml
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMlSourceProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductSourceProvider, MlSourceProvider>();
        }
    }
}
