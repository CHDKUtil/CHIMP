using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.Software.Sdm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSdmSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductBinarySoftwareDetector, SdmSoftwareDetector>();
        }

        public static IServiceCollection AddSdmAdHocSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductBinarySoftwareDetector, SdmAdHocSoftwareDetector>();
        }

        public static IServiceCollection AddSdmProductDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductDetector, SdmProductDetector>();
        }
    }
}
