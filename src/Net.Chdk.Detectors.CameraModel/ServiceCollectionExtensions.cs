using Microsoft.Extensions.DependencyInjection;
using System;

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

        [Obsolete]
        public static IServiceCollection AddMetadataCameraModelDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraModelDetector, MetadataCameraModelDetector>();
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
