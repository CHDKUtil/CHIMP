using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Boot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBootProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IBootProvider, BootProvider>();
        }

        public static IServiceCollection AddScriptProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IScriptProvider, ScriptProvider>();
        }
    }
}
