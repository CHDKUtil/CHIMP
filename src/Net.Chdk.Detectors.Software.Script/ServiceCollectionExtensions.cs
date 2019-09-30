using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Detectors.Software.Script
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScriptSoftwareDetector(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IInnerBinarySoftwareDetector, ScriptSoftwareDetector>();
        }
    }
}
