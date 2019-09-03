using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraTree.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonCameraTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraTreeProvider, JsonCameraTreeProvider>();
        }
    }
}
