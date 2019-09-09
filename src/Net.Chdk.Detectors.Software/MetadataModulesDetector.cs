using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using Net.Chdk.Validators;
using System;
using System.IO;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class MetadataModulesDetector : MetadataDetector<MetadataModulesDetector, ModulesInfo>, IInnerModulesDetector, IMetadataModulesDetector
    {
        public MetadataModulesDetector(IValidator<ModulesInfo> validator, ILoggerFactory loggerFactory)
            : base(validator, loggerFactory)
        {
        }

        public ModulesInfo GetModules(CardInfo card, CardInfo card2, SoftwareInfo software, IProgress<double> progress, CancellationToken token)
        {
            var rootPath = card.GetRootPath();
            var rootPath2 = card2?.GetRootPath();
            return GetModules(rootPath, rootPath2, software, progress, token);
        }

        public ModulesInfo GetModules(string basePath, string basePath2, SoftwareInfo software, IProgress<double> progress, CancellationToken token)
        {
            var productName = software.Product.Name;
            Logger.LogTrace("Detecting {0} modules from {1} metadata", productName, basePath);

            var filePath = Path.Combine(basePath, Directories.Metadata, software.Category.Name, FileName);
            var modules = GetValue(basePath2, filePath, progress, token);
            if (!productName.Equals(modules?.Product.Name, StringComparison.Ordinal))
                return null;

            return modules;
        }

        protected override string FileName => Files.Metadata.Modules;
    }
}
