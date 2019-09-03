using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Eos
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEosCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEosCameraWriter, EosCameraWriter>();
        }
    }
}
