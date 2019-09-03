using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.CameraTree.Csv
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCsvCameraTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerCameraTreeProvider, CsvCameraTreeProvider>();
        }
    }
}
