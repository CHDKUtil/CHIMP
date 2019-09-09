using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.Software.Chdk
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChdkSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductBinarySoftwareDetector, ChdkSoftwareDetector>();
        }

        public static IServiceCollection AddChdkProductDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductDetector, ChdkProductDetector>();
        }
    }
}
