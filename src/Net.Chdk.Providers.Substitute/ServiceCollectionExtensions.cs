using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Substitute
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSubstituteProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ISubstituteProvider, SubstituteProvider>();
        }
    }
}
