using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEosBuildProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEosBuildProvider, EosBuildProvider>()
                .AddSingleton<IEosCameraProvider, EosCameraProvider>()
                .AddSingleton<IEosCameraModelProvider, EosCameraModelProvider>()
                .AddSingleton<ICategoryEncodingProvider, EosEncodingProvider>()
                .AddSingleton<IVersionProvider, VersionProvider>();
        }
    }
}
