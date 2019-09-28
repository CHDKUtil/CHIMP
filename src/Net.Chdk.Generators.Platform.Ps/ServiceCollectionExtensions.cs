using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Generators.Platform.Ps
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPsPlatformGenerator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerPlatformGenerator, PsPlatformGenerator>()
                .AddSingleton<IInnerPlatformGenerator, IxusPlatformGenerator>()
                .AddSingleton<IIxusPlatformGenerator, IxusPlatformGenerator>()
                .AddSingleton<IInnerPlatformGenerator, PsEosPlatformGenerator>();
        }
    }
}
