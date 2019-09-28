using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Generators.Platform.Eos;
using Net.Chdk.Generators.Platform.Ps;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using Net.Chdk.Meta.Providers.Camera.Chdk;
using Net.Chdk.Meta.Providers.Camera.Eos;
using Net.Chdk.Meta.Providers.Camera.Fhp;
using Net.Chdk.Meta.Providers.Camera.Ml;
using Net.Chdk.Meta.Providers.Camera.Ps;
using Net.Chdk.Meta.Providers.Camera.Sdm;
using Net.Chdk.Meta.Providers.CameraList;
using Net.Chdk.Meta.Providers.CameraList.Csv;
using Net.Chdk.Meta.Providers.CameraList.Json;
using Net.Chdk.Meta.Providers.CameraList.Zip;
using Net.Chdk.Meta.Providers.CameraTree;
using Net.Chdk.Meta.Providers.CameraTree.Csv;
using Net.Chdk.Meta.Providers.CameraTree.Json;
using Net.Chdk.Meta.Providers.CameraTree.Src;
using Net.Chdk.Meta.Providers.Platform;
using Net.Chdk.Meta.Providers.Platform.Html;
using Net.Chdk.Meta.Providers.Platform.Xml;
using Net.Chdk.Meta.Providers.Sdm;
using Net.Chdk.Meta.Writers.Camera;
using Net.Chdk.Meta.Writers.Camera.Json;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Net.Chdk.Meta.Providers.Camera
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4 || args.Length > 5)
            {
                var name = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
                Console.WriteLine("Usage:");
                Console.WriteLine($"{name} product-name exiftool-meta camera-list [camera-tree] output");
                Console.WriteLine();
                Console.WriteLine("\tproduct-name   CHDK, SDM, ML or 400plus");
                Console.WriteLine("\tplatform-meta  exiftool.xml or Canon.html");
                Console.WriteLine("\tcamera-list    camera_list.csv, camera_list.json or ALL.zip");
                Console.WriteLine("\tcamera-tree    camera_list.csv, camera_list.json or CHDK source root");
                Console.WriteLine("\toutput         cameras.json");
                return;
            }

            var watch = Stopwatch.StartNew();

            var serviceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())

                .AddProductProvider()
                .AddBootProvider()

                .AddPlatformGenerator()
                .AddEosPlatformGenerator()
                .AddPsPlatformGenerator()

                .AddBuildProvider()
                .AddEosBuildProvider()
                .AddPsBuildProvider()

                .AddChdkCameraProviders()
                .AddSdmCameraProviders()
                .AddMlCameraProviders()
                .AddFhpCameraProviders()

                .AddCameraMetaProvider()
                .AddSdmCameraMetaProvider()

                .AddPlatformProvider()
                .AddHtmlPlatformProvider()
                .AddXmlPlatformProvider()

                .AddCameraListProvider()
                .AddCsvCameraListProvider()
                .AddJsonCameraListProvider()
                .AddZipCameraListProvider()

                .AddCameraTreeProvider()
                .AddCsvCameraTreeProvider()
                .AddJsonCameraTreeProvider()
                .AddSrcCameraTreeProvider()

                .AddCameraWriter()
                .AddJsonCameraWriter()

                .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            var productName = args[0];
            var platformPath = args[1];
            var listPath = args[2];
            var treePath = args.Length == 4
                ? listPath
                : args[3];
            var outPath = args[args.Length - 1];

            var platforms = GetPlatforms(serviceProvider, platformPath);
            var list = GetCameraList(serviceProvider, listPath, productName);
            var tree = GetCameraTree(serviceProvider, treePath);

            var cameras = GetCameras(serviceProvider, platforms, list, tree, productName);
            WriteCameras(serviceProvider, outPath, cameras);

            watch.Stop();
            logger.LogInformation("Completed in {0}", watch.Elapsed);
        }

        private static IDictionary<string, PlatformData> GetPlatforms(IServiceProvider serviceProvider, string path)
        {
            return serviceProvider.GetService<IPlatformProvider>()
                .GetPlatforms(path);
        }

        private static IDictionary<string, ListPlatformData> GetCameraList(IServiceProvider serviceProvider, string path, string productName)
        {
            return serviceProvider.GetService<ICameraListProvider>()
                .GetCameraList(path, productName);
        }

        private static IDictionary<string, TreePlatformData> GetCameraTree(IServiceProvider serviceProvider, string path)
        {
            return serviceProvider.GetService<ICameraTreeProvider>()
                .GetCameraTree(path);
        }

        private static IDictionary<string, CameraData> GetCameras(IServiceProvider serviceProvider, IDictionary<string, PlatformData> platforms,
            IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree, string productName)
        {
            return serviceProvider.GetService<IBuildProvider>()
                .GetCameras(platforms, list, tree, productName);
        }

        private static void WriteCameras(IServiceProvider serviceProvider, string path, IDictionary<string, CameraData> cameras)
        {
            serviceProvider.GetService<ICameraWriter>()
                .WriteCameras(path, cameras);
        }
    }
}
