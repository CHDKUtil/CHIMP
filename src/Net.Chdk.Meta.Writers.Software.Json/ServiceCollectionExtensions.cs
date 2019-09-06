using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Writers.Software.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonSoftwareWriter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISoftwareWriter, JsonSoftwareWriter>();
        }
    }
}
