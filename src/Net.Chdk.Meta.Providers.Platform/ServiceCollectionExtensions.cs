using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Platform
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlatformProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPlatformProvider, PlatformProvider>();
        }
    }
}
