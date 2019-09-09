using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.CameraModel
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraModelDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraModelDetector, CameraModelDetector>()
                .AddSingleton<IOuterCameraModelDetector, SoftwareCameraModelDetector>()
                .AddSingleton<IOuterCameraModelDetector, DerivedCameraModelDetector>();
        }

        public static IServiceCollection AddFileSystemCameraModelDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraModelDetector, FileSystemCameraModelDetector>();
        }

        public static IServiceCollection AddFileCameraModelDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IFileCameraModelDetector, FileCameraModelDetector>();
        }
    }
}
