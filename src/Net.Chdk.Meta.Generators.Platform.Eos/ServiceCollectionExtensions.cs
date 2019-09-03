using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Generators.Platform.Eos
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEosPlatformGenerator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerPlatformGenerator, EosPlatformGenerator>();
        }
    }
}
