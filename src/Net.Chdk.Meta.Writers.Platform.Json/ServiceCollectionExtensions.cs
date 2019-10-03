using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Platform.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonPlatformWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IPlatformWriter, JsonPlatformWriter>();
        }
    }
}
