using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPsBuildProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPsBuildProvider, PsBuildProvider>()
                .AddSingleton<IPsCameraProvider, PsCameraProvider>()
                .AddSingleton<IPsCameraModelProvider, PsCameraModelProvider>()
                .AddSingleton<ICategoryEncodingProvider, PsEncodingProvider>()
                .AddSingleton<IAltProvider, AltProvider>()
                .AddSingleton<IRevisionProvider, RevisionProvider>();
        }
    }
}
