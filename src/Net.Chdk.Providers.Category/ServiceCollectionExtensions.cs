using Microsoft.Extensions.DependencyInjection;
using System;

namespace Net.Chdk.Providers.Category
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCategoryProvider(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICategoryProvider, CategoryProvider>();
        }
    }
}
