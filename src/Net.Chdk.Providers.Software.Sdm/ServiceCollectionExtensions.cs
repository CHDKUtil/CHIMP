using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Software.Sdm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSdmSourceProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductSourceProvider, SdmSourceProvider>();
        }
    }
}
