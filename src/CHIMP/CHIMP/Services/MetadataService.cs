using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Detectors.Software;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using System;
using System.IO;
using System.Threading;

namespace Chimp.Services
{
    sealed class MetadataService : IMetadataService
    {
        private ILogger Logger { get; }
        private MainViewModel MainViewModel { get; }
        private IBootProvider BootProvider { get; }
        private IBinarySoftwareDetector BinarySoftwareDetector { get; }
        private IFileSystemModulesDetector FileSystemModulesDetector { get; }
        private IMetadataSoftwareDetector MetadataSoftwareDetector { get; }
        private IMetadataModulesDetector MetadaModulesDetector { get; }

        private SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);
        private CameraViewModel CameraViewModel => CameraViewModel.Get(MainViewModel);

        public MetadataService(MainViewModel mainViewModel, IBootProvider bootProvider, IBinarySoftwareDetector binarySoftwareDetector, IFileSystemModulesDetector fileSystemModulesDetector,
            IMetadataSoftwareDetector metadataSoftwareDetector, IMetadataModulesDetector metadataModulesDetector, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<MetadataService>();
            MainViewModel = mainViewModel;
            BootProvider = bootProvider;
            BinarySoftwareDetector = binarySoftwareDetector;
            FileSystemModulesDetector = fileSystemModulesDetector;
            MetadataSoftwareDetector = metadataSoftwareDetector;
            MetadaModulesDetector = metadataModulesDetector;
        }

        public SoftwareInfo Update(SoftwareInfo software, string destPath, IProgress<double> progress, CancellationToken token)
        {
            var updateSoftware = UpdateSoftware(ref software, destPath, progress, token);
            var updateModules = UpdateModules(software, out ModulesInfo modules, destPath, progress, token);

            var categoryName = software.Category.Name;
            var metadataPath = Path.Combine(destPath, Directories.Metadata, categoryName);
            Directory.CreateDirectory(metadataPath);
            if (updateSoftware)
                WriteMetadata(software, metadataPath, Files.Metadata.Software);
            if (updateModules)
                WriteMetadata(modules, metadataPath, Files.Metadata.Modules);

            return software;
        }

        private bool UpdateSoftware(ref SoftwareInfo software, string destPath, IProgress<double> progress, CancellationToken token)
        {
            var category = software.Category;
            var readSoftware = MetadataSoftwareDetector.GetSoftware(destPath, category, progress, token);
            if (readSoftware != null)
            {
                software = readSoftware;
                return false;
            }

            software.Encoding = SoftwareViewModel.SelectedItem?.Info.Encoding;

            var fileName = BootProvider.GetFileName(category.Name);
            var filePath = Path.Combine(destPath, fileName);
            var buffer = File.ReadAllBytes(filePath);
            BinarySoftwareDetector.UpdateSoftware(software, buffer);

            Logger.LogObject(LogLevel.Information, "Updated to {0}", software);

            return true;
        }

        private bool UpdateModules(SoftwareInfo software, out ModulesInfo modules, string destPath, IProgress<double> progress, CancellationToken token)
        {
            modules = MetadaModulesDetector.GetModules(destPath, destPath, software, progress, token);
            if (modules != null)
                return false;

            modules = FileSystemModulesDetector.GetModules(software, destPath, progress, token);

            Logger.LogObject(LogLevel.Information, "Detected {0}", modules);

            return true;
        }

        private void WriteMetadata<T>(T obj, string metadataPath, string fileName)
        {
            var filePath = Path.Combine(metadataPath, fileName);
            using (var stream = File.Create(filePath))
            {
                JsonObject.Serialize(stream, obj);
            }
        }
    }
}
