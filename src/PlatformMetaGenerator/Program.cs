using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Meta.Providers.Platform;
using Net.Chdk.Meta.Providers.Platform.Html;
using Net.Chdk.Meta.Writers.Platform;
using Net.Chdk.Meta.Writers.Platform.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AddressMetaGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Usage();
                return;
            }

            var serviceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())

                .AddPlatformGenerator()

                .AddPlatformProvider()
                .AddHtmlPlatformProvider()

                .AddJsonPlatformWriter()

                .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            var category = args[0];
            var inPath = args[1];
            var outPath = args[2];

            var platforms = GetPlatforms(serviceProvider, inPath, category);
            WritePlatforms(serviceProvider, outPath, platforms);
        }

        private static IDictionary<string, CameraModel[]> GetPlatforms(IServiceProvider serviceProvider, string path, string category)
        {
            return serviceProvider.GetService<IPlatformProvider>()
                .GetPlatforms(path, category);
        }

        private static void WritePlatforms(IServiceProvider serviceProvider, string path, IDictionary<string, CameraModel[]> platforms)
        {
            serviceProvider.GetService<IPlatformWriter>()
                .WritePlatforms(path, platforms);
        }

        private static void Usage()
        {
            var name = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            Console.WriteLine("Usage:");
            Console.WriteLine($"{name} category input output");
            Console.WriteLine();
            Console.WriteLine("\tcategory  Category name");
            Console.WriteLine("\tinput     Canon.html");
            Console.WriteLine("\toutput    platforms.json");
        }
    }
}
