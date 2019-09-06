using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Software.Sdm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSdmProductMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductMetaProvider, SdmProductMetaProvider>();
        }
    }
}
