using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Watchers.Volume
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVolumeWatcher(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IVolumeWatcher, VolumeWatcher>();
        }
    }
}
