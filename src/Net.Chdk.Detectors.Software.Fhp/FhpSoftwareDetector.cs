using Net.Chdk.Detectors.Software.Product;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System;
using System.Globalization;

namespace Net.Chdk.Detectors.Software.Fhp
{
    sealed class FhpSoftwareDetector : ProductBinarySoftwareDetector
    {
        private const string CameraPlatform = "400D";
        private const string CameraRevision = "111";

        public FhpSoftwareDetector(IProductProvider productProvider, ISourceProvider sourceProvider)
            : base(productProvider, sourceProvider)
        {
        }

        public override string ProductName => "400plus";

        protected override string String => "VER-";

        protected override int StringCount => 2;

        protected override bool GetProductVersion(string[] strings, out Version version, out string versionPrefix, out string versionSuffix)
        {
            version = null;
            versionPrefix = null;
            versionSuffix = null;

            if (!"%u".Equals(strings[1], StringComparison.Ordinal))
                return false;

            var split = strings[0].Split('-');
            if (split.Length != 2)
                return false;

            if (!DateTime.TryParseExact(split[0], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime date))
                return false;

            if ("BETA".Equals(split[1], StringComparison.Ordinal))
            {
                versionSuffix = "BETA";
                version = new Version(date.Year, date.Month, date.Day);
                return true;
            }

            if (!int.TryParse(split[1], out int revision))
                return false;

            version = new Version(date.Year, date.Month, date.Day, revision);
            return true;
        }

        protected override CultureInfo GetLanguage(string[] strings)
        {
            return GetCultureInfo("en");
        }

        protected override string GetPlatform(string[] strings)
        {
            return CameraPlatform;
        }

        protected override string GetRevision(string[] strings)
        {
            return CameraRevision;
        }
    }
}
