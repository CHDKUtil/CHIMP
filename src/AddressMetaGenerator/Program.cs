using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Net.Chdk.Adapters.Platform;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Address;
using Net.Chdk.Meta.Providers.Address.Json;
using Net.Chdk.Meta.Providers.Address.Src;
using Net.Chdk.Meta.Writers.Address;
using Net.Chdk.Meta.Writers.Address.Json;
using Net.Chdk.Providers.Firmware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AddressMetaGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var paths = ParseArgs(args).ToArray();
            if (paths.Length < 2)
            {
                Usage();
                return;
            }

            var serviceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())

                .AddFirmwareProvider()

                .AddPlatformAdapter()
                .AddChdkPlatformAdapter()

                .AddAddressTreeProvider()
                .AddJsonAddressTreeProvider()
                .AddSrcAddressTreeProvider()

                .AddAddressWriter()
                .AddJsonAddressWriter()

                .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            var addresses = GetAddressTree(serviceProvider, paths)
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            WriteAddresses(serviceProvider, paths[paths.Length - 1], addresses);
        }

        private static IEnumerable<KeyValuePair<string, AddressPlatformData>> GetAddressTree(IServiceProvider serviceProvider, string[] paths)
        {
            for (int i = 0; i < paths.Length - 1; i++)
            {
                var addresses = GetAddressTree(serviceProvider, paths[i]);
                foreach (var kvp in addresses)
                    yield return kvp;
            }
        }

        private static IDictionary<string, AddressPlatformData> GetAddressTree(IServiceProvider serviceProvider, string path)
        {
            return serviceProvider.GetService<IAddressTreeProvider>()
                .GetAddresses(path);
        }

        private static void WriteAddresses(IServiceProvider serviceProvider, string path, IDictionary<string, AddressPlatformData> addresses)
        {
            serviceProvider.GetService<IAddressWriter>()
                .WriteAddresses(path, addresses);
        }

        private static IEnumerable<string> ParseArgs(string[] args)
        {
            var outPath = "addresses.json";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Length > 0)
                {
                    if (args[i][0] == '-')
                    {
                        switch (args[i][1])
                        {
                            case 'o':
                                if (i == args.Length - 1)
                                    throw new InvalidOperationException("Missing output path");
                                outPath = args[++i];
                                break;
                            default:
                                throw new InvalidOperationException($"Unknown flag {args[i][1]}");
                        }
                    }
                    else
                    {
                        yield return args[i];
                    }
                }
            }
            yield return outPath;
        }

        private static void Usage()
        {
            var name = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            Console.WriteLine("Usage:");
            Console.WriteLine($"{name} input1 [input2 [...]] [-o output]");
            Console.WriteLine();
            Console.WriteLine("\tinput    CHDK source root or addresses.json");
            Console.WriteLine("\toutput   addresses.json");
        }
    }
}
