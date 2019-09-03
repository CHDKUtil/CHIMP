using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Eos.Props
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPropsEosCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEosInnerCameraWriter, PropsEosCameraWriter>();
        }
    }
}
