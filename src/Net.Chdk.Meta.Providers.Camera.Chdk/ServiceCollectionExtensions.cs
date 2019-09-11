using Microsoft.Extensions.DependencyInjection;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChdkCameraProviders(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductAltProvider, ChdkAltProvider>()
                .AddSingleton<IProductCameraCardProvider<PsCardData>, ChdkCameraCardProvider>()
                .AddSingleton<IProductCameraPlatformProvider, ChdkCameraPlatformProvider>()
                .AddSingleton<IProductCameraBootProvider, ChdkCameraBootProvider>()
                .AddSingleton<IProductCameraValidator, ChdkCameraValidator>()
                .AddSingleton<IProductCameraModelValidator, ChdkCameraModelValidator>();
        }
    }
}
