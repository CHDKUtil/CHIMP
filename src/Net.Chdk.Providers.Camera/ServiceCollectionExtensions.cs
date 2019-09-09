using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Camera
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraProvider, CameraProvider>();
        }
    }
}
