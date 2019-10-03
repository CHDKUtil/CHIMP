using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.CameraModel
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraModelProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraModelProvider, CameraModelProvider>();
        }
    }
}
