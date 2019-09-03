using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Platform.Xml
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddXmlPlatformProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerPlatformProvider, XmlPlatformProvider>();
        }
    }
}
