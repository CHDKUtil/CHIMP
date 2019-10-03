using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Adapters.Platform
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlatformAdapter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPlatformAdapter, PlatformAdapter>();
        }

        public static IServiceCollection AddChdkPlatformAdapter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductPlatformAdapter, ChdkPlatformAdapter>();
        }

        public static IServiceCollection AddSdmPlatformAdapter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductPlatformAdapter, SdmPlatformAdapter>();
        }

        public static IServiceCollection AddMlPlatformAdapter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductPlatformAdapter, MlPlatformAdapter>();
        }

        public static IServiceCollection AddFhpPlatformAdapter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductPlatformAdapter, FhpPlatformAdapter>();
        }
    }
}
