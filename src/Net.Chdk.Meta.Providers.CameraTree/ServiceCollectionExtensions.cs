using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraTree
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraTreeProvider, CameraTreeProvider>();
        }
    }
}
