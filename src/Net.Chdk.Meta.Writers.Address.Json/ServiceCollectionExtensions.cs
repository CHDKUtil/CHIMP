using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Address.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonAddressWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerAddressWriter, JsonAddressWriter>();
        }
    }
}
