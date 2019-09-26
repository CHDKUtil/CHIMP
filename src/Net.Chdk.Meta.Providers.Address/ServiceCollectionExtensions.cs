using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Address
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAddressTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IAddressTreeProvider, AddressTreeProvider>();
        }
    }
}
