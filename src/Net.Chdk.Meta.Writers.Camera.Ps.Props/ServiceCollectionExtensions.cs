using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Ps.Props
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPropsPsCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPsInnerCameraWriter, PropsPsCameraWriter>();
        }
    }
}
