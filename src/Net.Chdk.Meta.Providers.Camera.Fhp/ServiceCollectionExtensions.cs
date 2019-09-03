using Microsoft.Extensions.DependencyInjection;
using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Fhp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFhpCameraProviders(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductCameraPlatformProvider, FhpCameraPlatformProvider>()
                .AddSingleton<IProductCameraBootProvider, FhpCameraBootProvider>()
                .AddSingleton<IProductCameraCardProvider<EosCardData>, FhpCameraCardProvider>()
                .AddSingleton<IProductCameraValidator, FhpCameraValidator>()
                .AddSingleton<IProductCameraModelValidator, FhpCameraModelValidator>();
        }
    }
}
