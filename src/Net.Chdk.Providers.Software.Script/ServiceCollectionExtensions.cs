using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Software.Script
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClearOverlaysSourceProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IProductSourceProvider, ClearOverlaysSourceProvider>();
        }
    }
}
