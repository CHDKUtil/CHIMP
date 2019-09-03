using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Ps.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonPsCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPsInnerCameraWriter, JsonPsCameraWriter>();
        }
    }
}
