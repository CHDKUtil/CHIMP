using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraMetaProvider, CameraMetaProvider>();
        }
    }
}
