using Microsoft.Extensions.DependencyInjection;
using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEosBuildProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICategoryBuildProvider, EosBuildProvider>()
                .AddSingleton<ICameraProvider<EosCameraData, EosCameraModelData, VersionData, EosCardData>, EosCameraProvider>()
                .AddSingleton<ICategoryEncodingProvider, EosEncodingProvider>();
        }
    }
}
