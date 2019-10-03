using Microsoft.Extensions.DependencyInjection;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Sdm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSdmCameraProviders(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductAltProvider, SdmAltProvider>()
                .AddSingleton<IProductCameraCardProvider<PsCardData>, SdmCameraCardProvider>()
                .AddSingleton<IProductCameraBootProvider, SdmCameraBootProvider>()
                .AddSingleton<IProductCameraValidator, SdmCameraValidator>()
                .AddSingleton<IProductCameraModelValidator, SdmCameraModelValidator>();
        }
    }
}
