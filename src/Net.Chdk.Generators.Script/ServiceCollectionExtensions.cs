using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Generators.Script
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScriptGenerator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IScriptGenerator, ScriptGenerator>();
        }
    }
}
