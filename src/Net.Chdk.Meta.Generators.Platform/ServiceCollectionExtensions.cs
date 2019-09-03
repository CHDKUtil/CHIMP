using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Generators.Platform
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlatformGenerator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPlatformGenerator, PlatformGenerator>();
        }
    }
}
