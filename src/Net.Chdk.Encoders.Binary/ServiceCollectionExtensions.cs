using Microsoft.Extensions.DependencyInjection;

namespace Net.Chdk.Encoders.Binary
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBinaryEncoder(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IBinaryEncoder, BinaryEncoder>();
        }

        public static IServiceCollection AddBinaryDecoder(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IBinaryDecoder, BinaryDecoder>();
        }
    }
}
