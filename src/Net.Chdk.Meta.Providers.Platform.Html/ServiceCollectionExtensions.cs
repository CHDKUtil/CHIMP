using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Meta.Providers.Platform.Html
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHtmlPlatformProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerPlatformProvider, HtmlPlatformProvider>();
        }
    }
}
