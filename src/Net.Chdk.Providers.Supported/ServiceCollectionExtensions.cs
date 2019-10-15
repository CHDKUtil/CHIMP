using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Supported
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSupportedProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISupportedProvider, SupportedProvider>()
                .AddSingleton<IInnerSupportedProvider, SupportedErrorProvider>()
                .AddSingleton<IInnerSupportedProvider, SupportedBuildProvider>()
                .AddSingleton<IInnerSupportedProvider, SupportedRevisionProvider>()
                .AddSingleton<IInnerSupportedProvider, SupportedPlatformProvider>();
        }
    }
}
