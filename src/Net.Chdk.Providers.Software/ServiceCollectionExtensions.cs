using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Software
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSourceProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISourceProvider, SourceProvider>();
        }

        public static IServiceCollection AddSoftwareHashProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISoftwareHashProvider, SoftwareHashProvider>();
        }

        public static IServiceCollection AddModuleProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IModuleProvider, ModuleProvider>();
        }
    }
}
