using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software
{
    public abstract class SoftwareProvider : ISoftwareProvider
    {
        private static readonly Version Version = new Version("2.0");

        protected SoftwareSourceInfo Source { get; }
        private CultureInfo Language { get; }

        protected SoftwareProvider(SoftwareSourceInfo source, CultureInfo language)
        {
            Source = source;
            Language = language;
        }

        public SoftwareInfo GetSoftware(IMatchData? data, SoftwareInfo software)
        {
            var match = (data as MatchData)?.Payload?.LastOrDefault();
            return new SoftwareInfo
            {
                Version = Version,
                Category = software.Category,
                Source = Source,
                Product = GetProduct(match),
                Camera = software.Camera,
                Model = software.Model,
                Build = GetBuild(match),
            };
        }

        protected abstract string ProductName { get; }

        protected abstract Version? GetVersion(Match? match);

        protected virtual string? GetVersionPrefix(Match? match) => null;

        protected virtual string? GetVersionSuffix(Match? match) => null;

        private SoftwareProductInfo GetProduct(Match? match)
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

        private static SoftwareBuildInfo GetBuild(Match? match)
        {
            return new SoftwareBuildInfo
            {
                Name = match?.Groups["buildName"].Value,
                Status = match?.Groups["status"].Value.ToLowerInvariant(),
            };
        }

        protected static DateTime? GetCreationDate(Match? match)
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
