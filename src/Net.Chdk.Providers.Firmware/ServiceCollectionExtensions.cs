using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Providers.Firmware
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFirmwareProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IFirmwareProvider, FirmwareProvider>();
        }
    }
}
