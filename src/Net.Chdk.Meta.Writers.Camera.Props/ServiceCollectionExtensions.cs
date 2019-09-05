using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Props
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPropsCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraWriter, PropsCameraWriter>();
        }
    }
}
