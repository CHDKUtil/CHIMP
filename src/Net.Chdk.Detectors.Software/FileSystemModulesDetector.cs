using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Crypto;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class FileSystemModulesDetector : IInnerModulesDetector, IFileSystemModulesDetector
    {
        private static Version Version => new Version("1.0");

        private SoftwareDetectorSettings Settings { get; }
        private ILogger Logger { get; }
        private IModuleProvider ModuleProvider { get; }
        private IEnumerable<IInnerModuleDetector> ModuleDetectors { get; }
        private IHashProvider HashProvider { get; }

        private string HashName => Settings.HashName;

        public FileSystemModulesDetector(IModuleProvider moduleProvider, IEnumerable<IInnerModuleDetector> moduleDetectors, IHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILoggerFactory loggerFactory)
        {
            Settings = settings.Value;
            Logger = loggerFactory.CreateLogger<FileSystemModulesDetector>();
            ModuleProvider = moduleProvider;
            ModuleDetectors = moduleDetectors;
            HashProvider = hashProvider;
        }

        public ModulesInfo GetModules(CardInfo card, CardInfo card2, SoftwareInfo software, IProgress<double> progress, CancellationToken token)
        {
            if (card2 == null)
                return null;
            var rootPath = card2.GetRootPath();
            return GetModules(software, rootPath, progress, token);
        }

        public ModulesInfo GetModules(SoftwareInfo software, string basePath, IProgress<double> progress, CancellationToken token)
        {
            var productName = software.Product.Name;
            Logger.LogTrace("Detecting {0} modules from {1} file system", productName, basePath);

            return new ModulesInfo
            {
                Version = new Version("1.0"),
                Product = new ModulesProductInfo
                {
                    Name = productName
                },
                Modules = DoGetModules(software, basePath, progress, token)
            };
        }

        private Dictionary<string, ModuleInfo> DoGetModules(SoftwareInfo software, string basePath, IProgress<double> progress, CancellationToken token)
        {
            var productName = software.Product.Name;
            var modulesPath = ModuleProvider.GetPath(productName);
            if (modulesPath == null)
                return null;

            var path = Path.Combine(basePath, modulesPath);
            if (!Directory.Exists(path))
                return null;

            token.ThrowIfCancellationRequested();

            var extension = ModuleProvider.GetExtension(productName);
            var pattern = string.Format("*{0}", extension);
            var files = Directory.EnumerateFiles(path, pattern);
            var count = progress != null
                ? files.Count()
                : 0;
            var index = 0;
            var modules = new Dictionary<string, ModuleInfo>();
            foreach (var file in files)
            {
                token.ThrowIfCancellationRequested();

                AddFile(software, modulesPath, file, modules);
                if (progress != null)
                    progress.Report((double)(++index) / count);
            }
            return modules;
        }

        private void AddFile(SoftwareInfo software, string modulesPath, string file, Dictionary<string, ModuleInfo> modules)
        {
            var fileName = Path.GetFileName(file);
            var filePath = Path.Combine(modulesPath, fileName).ToLowerInvariant();

            var productName = software.Product.Name;
            var moduleName = ModuleProvider.GetModuleName(productName, filePath);
            if (moduleName == null)
            {
                Logger.LogError("Missing module for {0}", filePath);
                moduleName = fileName;
            }

            var buffer = File.ReadAllBytes(file);

            if (!modules.TryGetValue(moduleName, out ModuleInfo moduleInfo))
            {
                moduleInfo = GetModule(software, buffer);
                modules.Add(moduleName, moduleInfo);
            }

            var hashString = HashProvider.GetHashString(buffer, HashName);
            moduleInfo.Hash.Values.Add(filePath, hashString);
        }

        private ModuleInfo GetModule(SoftwareInfo software, byte[] buffer)
        {
            return ModuleDetectors
                .Select(d => d.GetModule(software, buffer, HashName))
                .FirstOrDefault(m => m != null);
        }
    }
}
