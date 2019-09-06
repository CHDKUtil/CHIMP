using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Software.Zip
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddZipSoftwareMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISoftwareMetaProvider, ZipSoftwareMetaProvider>();
        }
    }
}
