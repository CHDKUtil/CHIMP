using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.Software.Ml
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNightlyMlSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductBinarySoftwareDetector, NightlyMlSoftwareDetector>();
        }

        public static IServiceCollection AddBetaMlSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductBinarySoftwareDetector, BetaMlSoftwareDetector>();
        }

        public static IServiceCollection AddMlModuleDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductBinaryModuleDetector, MlModuleDetector>();
        }

        public static IServiceCollection AddMlProductDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductDetector, MlProductDetector>();
        }
    }
}
