using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Software
{
    abstract class SoftwareProvider : ISoftwareProvider
    {
        private static readonly Version Version = new Version("2.0");

        private IProductProvider ProductProvider { get; }
        protected SoftwareSourceInfo Source { get; }
        private CultureInfo Language { get; }

        protected SoftwareProvider(IProductProvider productProvider, SoftwareSourceInfo source, CultureInfo language)
        {
            ProductProvider = productProvider;
            Source = source;
            Language = language;
        }

        public SoftwareInfo GetSoftware(Match match, SoftwareInfo software)
        {
            return new SoftwareInfo
            {
                Version = Version,
                Category = GetCategory(match),
                Source = Source,
                Product = GetProduct(match),
                Camera = software.Camera,
                Model = software.Model,
                Build = GetBuild(match),
            };
        }

        protected abstract string ProductName { get; }

        private string CategoryName => ProductProvider.GetCategoryName(ProductName);

        protected abstract Version GetVersion(Match match);

        protected virtual string GetVersionPrefix(Match match) => null;

        protected virtual string GetVersionSuffix(Match match) => null;

        private CategoryInfo GetCategory(Match match)
        {
            return new CategoryInfo
            {
                Name = CategoryName
            };
        }

        private SoftwareProductInfo GetProduct(Match match)
        {
            return new SoftwareProductInfo
            {
                Name = ProductName,
                Language = Language,
                Version = GetVersion(match),
                VersionPrefix = GetVersionPrefix(match),
                VersionSuffix = GetVersionSuffix(match),
                Created = GetCreationDate(match),
            };
        }

        private static SoftwareBuildInfo GetBuild(Match match)
        {
            return new SoftwareBuildInfo
            {
                Name = match?.Groups["buildName"].Value,
                Status = match?.Groups["status"].Value.ToLowerInvariant(),
            };
        }

        protected static DateTime? GetCreationDate(Match match)
        {
            if (match == null)
                return null;
            var created = match.Groups["date"].Value;
            if (created.Length == 0)
                return null;
            return DateTime.Parse(created, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }
    }
}
