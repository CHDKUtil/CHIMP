using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class BinarySoftwareDetector : IInnerSoftwareDetector, IBinarySoftwareDetector
    {
        private ILogger Logger { get; }

        private IEnumerable<IInnerBinarySoftwareDetector> SoftwareDetectors { get; }

        public BinarySoftwareDetector(IEnumerable<IInnerBinarySoftwareDetector> softwareDetectors, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<BinarySoftwareDetector>();
            SoftwareDetectors = softwareDetectors;
        }

        public SoftwareInfo GetSoftware(CardInfo cardInfo, CategoryInfo category, IProgress<double> progress, CancellationToken token)
        {
            var baseBath = cardInfo.GetRootPath();
            return GetSoftware(baseBath, category.Name, progress, token);
        }

        public SoftwareInfo GetSoftware(string basePath, string categoryName, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting {0} software from {1} binaries", categoryName, basePath);

            return SoftwareDetectors
                .Select(d => d.GetSoftware(basePath, categoryName, progress, token))
                .FirstOrDefault(s => s != null);
        }

        public SoftwareInfo GetSoftware(byte[] buffer, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting software from buffer");

            return SoftwareDetectors
                .Select(d => d.GetSoftware(buffer, progress, token))
                .FirstOrDefault(s => s != null);
        }

        public bool UpdateSoftware(SoftwareInfo software, byte[] buffer)
        {
            var productName = software.Product.Name;
            Logger.LogTrace("Updating {0}", productName);

            return SoftwareDetectors
                .Any(d => d.UpdateSoftware(software, buffer));
        }
    }
}
