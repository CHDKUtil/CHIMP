using Net.Chdk.Detectors.Software.Product;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System;
using System.Globalization;
using System.Linq;

namespace Net.Chdk.Detectors.Software.Ml
{
    abstract class MlSoftwareDetector : ProductBinarySoftwareDetector
    {
        protected MlSoftwareDetector(IProductProvider productProvider, ISourceProvider sourceProvider)
            : base(productProvider, sourceProvider)
        {
        }

        public sealed override string ProductName => "ML";

        protected sealed override bool GetProductVersion(string[] strings, out Version version, out string versionPrefix, out string versionSuffix)
        {
            version = null;
            versionPrefix = null;
            versionSuffix = null;
            var split = GetVersionString(strings).Split('.');
            if (split.Length < 3)
                return false;
            var versionStr = split[split.Length - 2];
            if (!DateTime.TryParse(versionStr, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date))
                return false;
            version = new Version(date.Year, date.Month, date.Day);
            versionPrefix = string.Join(".", split.Take(split.Length - 2));
            return true;
        }

        protected override Version GetProductVersion(string[] strings)
        {
            throw new NotImplementedException();
        }

        protected sealed override DateTime? GetCreationDate(string[] strings)
        {
            var dateStr = GetCreationDateString(strings);
            if (!DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date))
                return null;
            return date;
        }

        protected sealed override SoftwareBuildInfo GetBuild(string[] strings)
        {
            return new SoftwareBuildInfo
            {
                Name = string.Empty,
                Status = GetStatus(strings),
                Changeset = GetChangeset(strings),
                Creator = GetCreator(strings),
            };
        }

        protected sealed override CultureInfo GetLanguage(string[] strings)
        {
            return GetCultureInfo("en");
        }

        protected abstract string GetVersionString(string[] strings);
        protected abstract string GetCreationDateString(string[] strings);
        protected abstract string GetStatus(string[] strings);
        protected abstract string GetChangeset(string[] strings);
        protected abstract string GetCreator(string[] strings);
    }
}
