using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Eos.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonEosCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEosInnerCameraWriter, JsonEosCameraWriter>();
        }
    }
}
