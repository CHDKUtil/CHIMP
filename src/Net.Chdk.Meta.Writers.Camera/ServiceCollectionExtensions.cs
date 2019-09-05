using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraWriter, CameraWriter>();
        }
    }
}
