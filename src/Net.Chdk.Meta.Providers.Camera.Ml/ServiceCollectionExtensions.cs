using Microsoft.Extensions.DependencyInjection;
using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Ml
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMlCameraProviders(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductCameraPlatformProvider, MlCameraPlatformProvider>()
                .AddSingleton<IProductCameraBootProvider, MlCameraBootProvider>()
                .AddSingleton<IProductCameraCardProvider<EosCardData>, MlCameraCardProvider>()
                .AddSingleton<IProductCameraValidator, MlCameraValidator>()
                .AddSingleton<IProductCameraModelValidator, MlCameraModelValidator>();
        }
    }
}
