using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class SoftwareDetector : ISoftwareDetector
    {
        private ILogger Logger { get; }
        private IProductProvider ProductProvider { get; }
        private IEnumerable<IInnerSoftwareDetector> SoftwareDetectors { get; }

        public SoftwareDetector(IProductProvider productProvider, IEnumerable<IInnerSoftwareDetector> softwareDetectors, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<SoftwareDetector>();
            ProductProvider = productProvider;
            SoftwareDetectors = softwareDetectors;

            categories = new Lazy<IEnumerable<CategoryInfo>>(GetCategories);
        }

        public SoftwareInfo[] GetSoftware(CardInfo cardInfo, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting software from {0}", cardInfo.DriveLetter);

            return Categories
                .Select(c => GetSoftware(cardInfo, c, progress, token))
                .Where(s => s != null)
                .ToArray();
        }

        private SoftwareInfo GetSoftware(CardInfo cardInfo, CategoryInfo category, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting {0} software from {1}", category.Name, cardInfo.DriveLetter);

            return SoftwareDetectors
                .Select(d => d.GetSoftware(cardInfo, category, progress, token))
                .FirstOrDefault(s => s != null);
        }

        #region Categories

        private readonly Lazy<IEnumerable<CategoryInfo>> categories;

        private IEnumerable<CategoryInfo> Categories => categories.Value;

        private IEnumerable<CategoryInfo> GetCategories()
        {
            return ProductProvider.GetCategoryNames()
                .Select(CreateCategory);
        }

        private static CategoryInfo CreateCategory(string categoryName)
        {
            return new CategoryInfo
            {
                Name = categoryName
            };
        }

        #endregion
    }
}
