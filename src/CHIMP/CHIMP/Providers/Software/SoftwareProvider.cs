using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Software
{
    abstract class SoftwareProvider : ISoftwareProvider
    {
        private static readonly Version Version = new Version("1.0");

        protected SoftwareSourceInfo Source { get; }
        private CultureInfo Language { get; }

        protected SoftwareProvider(SoftwareSourceInfo source, CultureInfo language)
        {
            Source = source;
            Language = language;
        }

        public SoftwareInfo GetSoftware(Match match)
        {
            return new SoftwareInfo
            {
                Version = Version,
                Category = GetCategory(match),
                Source = Source,
                Product = GetProduct(match),
                Camera = GetCamera(match),
                Build = GetBuild(match),
            };
        }

        protected abstract string ProductName { get; }

        protected abstract string CategoryName { get; }

        protected abstract Version GetVersion(Match match);

        protected virtual string GetVersionPrefix(Match match) => null;

        private CategoryInfo GetCategory(Match match)
        {
            return new CategoryInfo
            {
                Name = CategoryName,
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
                Created = GetCreationDate(match),
            };
        }

        private static SoftwareCameraInfo GetCamera(Match match)
        {
            return new SoftwareCameraInfo
            {
                Platform = match.Groups["platform"].Value,
                Revision = match.Groups["revision"].Value,
            };
        }

        private static SoftwareBuildInfo GetBuild(Match match)
        {
            return new SoftwareBuildInfo
            {
                Name = match.Groups["buildName"].Value,
                Status = match.Groups["status"].Value.ToLowerInvariant(),
            };
        }

        protected static DateTime? GetCreationDate(Match match)
        {
            var created = match.Groups["date"].Value;
            if (created.Length == 0)
                return null;
            return DateTime.Parse(created, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }
    }
}
