using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Camera.Ps
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPsCameraWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPsCameraWriter, PsCameraWriter>();
        }
    }
}
