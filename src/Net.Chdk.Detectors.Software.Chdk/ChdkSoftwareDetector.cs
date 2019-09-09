using Net.Chdk.Detectors.Software.Product;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System;
using System.Globalization;
using System.Text;

namespace Net.Chdk.Detectors.Software.Chdk
{
    sealed class ChdkSoftwareDetector : ProductBinarySoftwareDetector
    {
        private static readonly string[] Prefixes = new[]
        {
            "Version ",
            "Firmware ",
        };

        private static byte[][] PrefixBytes;

        static ChdkSoftwareDetector()
        {
            PrefixBytes = new byte[Prefixes.Length][];
            for (var i = 0; i < PrefixBytes.Length; i++)
                PrefixBytes[i] = Encoding.ASCII.GetBytes(Prefixes[i]);
        }

        public ChdkSoftwareDetector(IProductProvider productProvider, ISourceProvider sourceProvider)
            : base(productProvider, sourceProvider)
        {
        }

        public override string ProductName => "CHDK";

        protected override string String => "CHDK ";
        protected override int StringCount => 4;

        public override SoftwareInfo GetSoftware(byte[] buffer, int index)
        {
            for (var i = 0; i < PrefixBytes.Length; i++)
                if (Equals(buffer, PrefixBytes[i], index))
                    return base.GetSoftware(buffer, index + PrefixBytes[i].Length);
            return null;
        }

        protected override Version GetProductVersion(string[] strings)
        {
            var split = strings[0].Trim('\'').Split(' ');
            if (split.Length != 2)
                return null;
            var str = GetVersionString(split[1].Split('-'));
            return GetVersion(str);
        }

        protected override CultureInfo GetLanguage(string[] strings)
        {
            var sourceName = GetSourceName(strings);
            switch (sourceName)
            {
                case "CHDK":
                    return GetCultureInfo("en");
                case "CHDK_DE":
                    return GetCultureInfo("de");
                default:
                    return null;
            }
        }

        protected override DateTime? GetCreationDate(string[] strings)
        {
            var str = strings[1].TrimStart("Build: ");
            return GetCreationDate(str);
        }

        protected override SoftwareCameraInfo GetCamera(string[] strings)
        {
            var str = strings[2].TrimStart("Camera: ");
            if (str == null)
                return null;
            var split = str.Split(new[] { " - " }, StringSplitOptions.None);
            if (split.Length != 2)
                return null;
            return GetCamera(split[0], split[1]);
        }

        protected override string GetSourceName(string[] strings)
        {
            return strings[0].Trim('\'').Split(' ')[0];
        }

        protected override SoftwareBuildInfo GetBuild(string[] strings)
        {
            return new SoftwareBuildInfo
            {
                Changeset = GetChangeset(strings),
            };
        }

        protected override SoftwareCompilerInfo GetCompiler(string[] strings)
        {
            var split = strings[3].Split(' ');
            if (split.Length != 2)
                return null;
            if (!Version.TryParse(split[1], out Version version))
                return null;
            return new SoftwareCompilerInfo
            {
                Name = split[0],
                Version = version
            };
        }

        private string GetChangeset(string[] strings)
        {
            var version = GetProductVersion(strings);
            return version?.MinorRevision.ToString();
        }

        private static string GetVersionString(string[] versionSplit)
        {
            switch (versionSplit.Length)
            {
                case 0:
                    return null;
                case 1:
                    return versionSplit[0];
                default:
                    return string.Join(".", versionSplit[0], versionSplit[1]);
            }
        }

        private static bool Equals(byte[] buffer, byte[] bytes, int start)
        {
            for (var j = 0; j < bytes.Length; j++)
                if (buffer[start + j] != bytes[j])
                    return false;
            return true;
        }
    }
}
