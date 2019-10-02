using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPsPlatformGenerator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerPlatformGenerator, PsPlatformGenerator>()
                .AddSingleton<IInnerPlatformGenerator, IxusPlatformGenerator>()
                .AddSingleton<IxusPlatformGenerator>()
                .AddSingleton<IInnerPlatformGenerator, PsEosPlatformGenerator>();
        }
    }
}
