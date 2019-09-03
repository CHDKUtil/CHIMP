using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSrcCameraTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraTreeProvider, SrcCameraTreeProvider>()
                .AddSingleton<SourceProvider>()
                .AddSingleton<IdProvider>()
                .AddSingleton<EncodingProvider>()
                .AddSingleton<PlatformProvider>()
                .AddSingleton<CameraProvider>()
                .AddSingleton<RevisionProvider>();
        }
    }
}
