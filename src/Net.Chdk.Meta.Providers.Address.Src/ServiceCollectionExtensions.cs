using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSrcAddressTreeProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerAddressTreeProvider, SrcAddressTreeProvider>()
                .AddSingleton<SourceProvider>()
                .AddSingleton<DataProvider>()
                .AddSingleton<AddressProvider>()
                .AddSingleton<PlatformProvider>()
                .AddSingleton<CameraProvider>()
                .AddSingleton<StubsDataProvider>()
                .AddSingleton<RevisionAddressProvider>()
                .AddSingleton<RevisionProvider>();
        }
    }
}
