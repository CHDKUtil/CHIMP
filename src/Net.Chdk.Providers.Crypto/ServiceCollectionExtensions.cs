using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Crypto
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHashProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IHashProvider, HashProvider>();
        }
    }
}
