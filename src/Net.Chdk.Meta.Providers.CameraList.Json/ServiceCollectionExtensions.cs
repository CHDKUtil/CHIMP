using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraList.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonCameraListProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraListProvider, JsonCameraListProvider>();
        }
    }
}
