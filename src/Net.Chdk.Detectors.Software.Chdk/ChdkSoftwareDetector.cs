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

        private static readonly byte[][] PrefixBytes;

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

        public override SoftwareInfo? GetSoftware(byte[] buffer, int index)
        {
            for (var i = 0; i < PrefixBytes.Length; i++)
                if (Equals(buffer, PrefixBytes[i], index))
                    return base.GetSoftware(buffer, index + PrefixBytes[i].Length);
            return null;
        }

        protected override Version? GetProductVersion(string?[] strings)
        {
            var split = GetProductSplit(strings);
            if (split?.Length != 2)
                return null;
            var str = GetVersionString(split[1].Split('-'));
            return GetVersion(str);
        }

        protected override CultureInfo? GetLanguage(string?[] strings)
        {
            var sourceName = GetSourceName(strings);
            return sourceName switch
            {
                "CHDK" => GetCultureInfo("en"),
                "CHDK_DE" => GetCultureInfo("de"),
                _ => null,
            };
        }

        protected override DateTime? GetCreationDate(string?[] strings)
        {
            var str = strings[1].TrimStart("Build: ");
            return GetCreationDate(str);
        }

        protected override SoftwareCameraInfo? GetCamera(string?[] strings)
        {
            var cameraStr = strings[2];
            if (cameraStr == null)
                return null;
            var str = cameraStr.TrimStart("Camera: ");
            var split = str!.Split(new[] { " - " }, StringSplitOptions.None);
            if (split.Length != 2)
                return null;
            return GetCamera(split[0], split[1]);
        }

        protected override string? GetSourceName(string?[] strings)
        {
            var split = GetProductSplit(strings);
            return split?[0];
        }

        protected override SoftwareBuildInfo GetBuild(string?[] strings)
        {
            return new SoftwareBuildInfo
            {
                Changeset = GetChangeset(strings),
            };
        }

        protected override SoftwareCompilerInfo? GetCompiler(string?[] strings)
        {
            var compilerStr = strings[3];
            if (compilerStr == null)
                return null;
            var split = compilerStr.Split(' ');
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

        private static string[]? GetProductSplit(string?[] strings)
        {
            var productStr = strings[0];
            if (productStr == null)
                return null;
            return productStr.Trim('\'').Split(' ');
        }

        private string? GetChangeset(string?[] strings)
        {
            var version = GetProductVersion(strings);
            return version?.MinorRevision.ToString();
        }

        private static string? GetVersionString(string[] versionSplit)
        {
            return versionSplit.Length switch
            {
                0 => null,
                1 => versionSplit[0],
                _ => string.Join(".", versionSplit[0], versionSplit[1]),
            };
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
