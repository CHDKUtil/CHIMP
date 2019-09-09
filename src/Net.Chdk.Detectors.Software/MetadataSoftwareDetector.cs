using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Validators;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class MetadataSoftwareDetector : MetadataDetector<MetadataSoftwareDetector, SoftwareInfo>, IInnerSoftwareDetector, IMetadataSoftwareDetector
    {
        public MetadataSoftwareDetector(IValidator<SoftwareInfo> validator, ILoggerFactory loggerFactory)
            : base(validator, loggerFactory)
        {
        }

        public SoftwareInfo GetSoftware(CardInfo card, CategoryInfo category, IProgress<double> progress, CancellationToken token)
        {
            var basePath = card.GetRootPath();
            return GetSoftware(basePath, category, progress, token);
        }

        public SoftwareInfo GetSoftware(string basePath, CategoryInfo category, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting {0} software from {1} metadata", category.Name, basePath);

            return GetValue(basePath, category, progress, token);
        }

        protected override string FileName => Files.Metadata.Software;
    }
}
