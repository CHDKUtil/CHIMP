using Microsoft.Extensions.DependencyInjection;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Validators.Software
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSoftwareHashValidator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IValidator<SoftwareHashInfo>, SoftwareHashValidator>();
        }

        public static IServiceCollection AddSoftwareValidator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IValidator<SoftwareInfo>, SoftwareValidator>();
        }

        public static IServiceCollection AddModulesValidator(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IValidator<ModulesInfo>, ModulesValidator>();
        }
    }
}
