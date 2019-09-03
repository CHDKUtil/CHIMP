using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraList.Csv
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCsvCameraListProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraListProvider, CsvCameraListProvider>();
        }
    }
}
