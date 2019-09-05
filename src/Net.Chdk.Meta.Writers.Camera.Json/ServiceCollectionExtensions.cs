using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraWriter, JsonCameraWriter>();
        }
    }
}
