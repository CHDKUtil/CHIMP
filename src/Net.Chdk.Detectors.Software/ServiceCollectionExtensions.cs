using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.Software
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISoftwareDetector, SoftwareDetector>();
        }

        public static IServiceCollection AddMetadataSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerSoftwareDetector, MetadataSoftwareDetector>()
                .AddSingleton<IMetadataSoftwareDetector, MetadataSoftwareDetector>();
        }

        public static IServiceCollection AddBinarySoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerSoftwareDetector, BinarySoftwareDetector>()
                .AddSingleton<IBinarySoftwareDetector, BinarySoftwareDetector>();
        }

        public static IServiceCollection AddEosHashSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerBinarySoftwareDetector, EosHashSoftwareDetector>();
        }

        public static IServiceCollection AddPsHashSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerBinarySoftwareDetector, PsHashSoftwareDetector>();
        }

        public static IServiceCollection AddEosBinarySoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerBinarySoftwareDetector, EosBinarySoftwareDetector>();
        }

        public static IServiceCollection AddKnownPsBinarySoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerBinarySoftwareDetector, KnownPsBinarySoftwareDetector>();
        }

        public static IServiceCollection AddUnkownPsBinarySoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerBinarySoftwareDetector, UnknownPsBinarySoftwareDetector>();
        }

        public static IServiceCollection AddFileSystemSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerSoftwareDetector, FileSystemSoftwareDetector>();
        }

        public static IServiceCollection AddModulesDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IModulesDetector, ModulesDetector>();
        }

        public static IServiceCollection AddMetadataModulesDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerModulesDetector, MetadataModulesDetector>()
                .AddSingleton<IMetadataModulesDetector, MetadataModulesDetector>();
        }

        public static IServiceCollection AddFileSystemModulesDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerModulesDetector, FileSystemModulesDetector>()
                .AddSingleton<IFileSystemModulesDetector, FileSystemModulesDetector>();
        }

        public static IServiceCollection AddBinaryModuleDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerModuleDetector, BinaryModuleDetector>();
        }

        public static IServiceCollection AddDerivedModuleDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerModuleDetector, DerivedModuleDetector>();
        }
    }
}
