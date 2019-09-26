using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Address.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonAddressTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerAddressTreeProvider, JsonAddressTreeProvider>();
        }
    }
}
