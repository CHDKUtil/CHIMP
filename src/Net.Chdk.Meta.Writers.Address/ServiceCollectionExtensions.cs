using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Address
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAddressWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IAddressWriter, AddressWriter>();
        }
    }
}
