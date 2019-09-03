using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Camera
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBuildProviders(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEncodingProvider, EncodingProvider>()
                .AddSingleton<ICameraPlatformProvider, CameraPlatformProvider>()
                .AddSingleton<ICameraBootProvider, CameraBootProvider>()
                .AddSingleton<ICameraValidator, CameraValidator>()
                .AddSingleton<ICameraModelValidator, CameraModelValidator>()
                .AddSingleton(typeof(ICameraCardProvider<>), typeof(CameraCardProvider<>));
        }
    }
}
