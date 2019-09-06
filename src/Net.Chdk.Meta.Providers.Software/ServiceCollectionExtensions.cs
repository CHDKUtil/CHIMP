using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Software
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCategoryMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICategoryMetaProvider, CategoryMetaProvider>();
        }

        public static IServiceCollection AddSourceMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISourceMetaProvider, SourceMetaProvider>();
        }

        public static IServiceCollection AddBuildMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IBuildMetaProvider, BuildMetaProvider>();
        }

        public static IServiceCollection AddCompilerMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICompilerMetaProvider, CompilerMetaProvider>();
        }

        public static IServiceCollection AddEncodingMetaProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEncodingMetaProvider, EncodingMetaProvider>();
        }
    }
}
