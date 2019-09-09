using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.Camera
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCameraDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICameraDetector, CameraDetector>();
        }

        public static IServiceCollection AddFileSystemCameraDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraDetector, FileSystemCameraDetector>()
                .AddSingleton<IFilePatternProvider, FileSystemCameraDetector>();
        }

        public static IServiceCollection AddAllFileSystemCameraDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraDetector, AllFileSystemCameraDetector>()
                .AddSingleton<IFilePatternProvider, AllFileSystemCameraDetector>();
        }

        public static IServiceCollection AddFileCameraDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IFileCameraDetector, FileCameraDetector>();
        }
    }
}
