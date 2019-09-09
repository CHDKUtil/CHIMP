using Microsoft.Extensions.DependencyInjection;
using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPsBuildProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICategoryBuildProvider, PsBuildProvider>()
                .AddSingleton<ICameraProvider<PsCameraData, PsCameraModelData, PsRevisionData, PsCardData>, PsCameraProvider>()
                .AddSingleton<IEncodingProvider, EncodingProvider>()
                .AddSingleton<IAltProvider, AltProvider>();
        }
    }
}
