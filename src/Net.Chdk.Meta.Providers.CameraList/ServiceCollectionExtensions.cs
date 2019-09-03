using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraList
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraListProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraListProvider, CameraListProvider>();
        }
    }
}
