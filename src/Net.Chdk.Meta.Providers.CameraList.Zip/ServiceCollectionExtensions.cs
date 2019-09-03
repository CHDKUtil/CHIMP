using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraList.Zip
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddZipCameraListProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraListProvider, ZipCameraListProvider>();
        }
    }
}
