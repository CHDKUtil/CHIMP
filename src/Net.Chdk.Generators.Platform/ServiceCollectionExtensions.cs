using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Generators.Platform
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlatformGenerator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPlatformGenerator, PlatformGenerator>()
                .AddSingleton<IInnerPlatformGenerator, EosPlatformGenerator>()
                .AddSingleton<IInnerPlatformGenerator, IxusPlatformGenerator>()
                .AddSingleton<IInnerPlatformGenerator, PsPlatformGenerator>()
                ;
        }
    }
}
