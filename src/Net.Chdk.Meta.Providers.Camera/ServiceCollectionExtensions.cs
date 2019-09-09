using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Camera
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBuildProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IBuildProvider, BuildProvider>()
                .AddSingleton<ICameraPlatformProvider, CameraPlatformProvider>()
                .AddSingleton<ICameraBootProvider, CameraBootProvider>()
                .AddSingleton<ICameraValidator, CameraValidator>()
                .AddSingleton<ICameraModelValidator, CameraModelValidator>()
                .AddSingleton(typeof(IRevisionProvider<>), typeof(RevisionProvider<>))
                .AddSingleton(typeof(ICameraModelProvider<,>), typeof(CameraModelProvider<,>))
                .AddSingleton(typeof(ICameraCardProvider<>), typeof(CameraCardProvider<>));
        }
    }
}
