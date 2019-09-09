using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.Software.Fhp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFhpSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductBinarySoftwareDetector, FhpSoftwareDetector>();
        }

        public static IServiceCollection AddFhpProductDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductDetector, FhpProductDetector>();
        }
    }
}
