using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Sdm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSdmCameraMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductCameraMetaProvider, SdmCameraMetaProvider>();
        }
    }
}
