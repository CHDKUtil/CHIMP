using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class FileSystemSoftwareDetector : IInnerSoftwareDetector
    {
        private static Version Version => new Version("1.0");

        private ILogger Logger { get; }
        private IEnumerable<IProductDetector> ProductDetectors { get; }
        private IBootProvider BootProvider { get; }

        public FileSystemSoftwareDetector(IEnumerable<IProductDetector> productDetectors, IBootProvider bootProvider, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<FileSystemSoftwareDetector>();
            ProductDetectors = productDetectors;
            BootProvider = bootProvider;
        }

        public SoftwareInfo GetSoftware(CardInfo cardInfo, CategoryInfo category, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting {0} software from {1} file system", category.Name, cardInfo.DriveLetter);

            var rootPath = cardInfo.GetRootPath();
            var fileName = BootProvider.GetFileName(category.Name);
            var filePath = Path.Combine(rootPath, fileName);
            if (!File.Exists(filePath))
                return null;
            return new SoftwareInfo
            {
                Version = Version,
                Category = category,
                Product = GetProduct(cardInfo, category.Name),
            };
        }

        private SoftwareProductInfo GetProduct(CardInfo cardInfo, string categoryName)
        {
            return ProductDetectors
                .Where(d => categoryName.Equals(d.CategoryName, StringComparison.Ordinal))
                .Select(d => d.GetProduct(cardInfo))
                .FirstOrDefault(p => p != null);
        }
    }
}
