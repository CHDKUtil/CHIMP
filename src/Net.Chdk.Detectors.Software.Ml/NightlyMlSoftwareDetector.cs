using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System.Linq;

namespace Net.Chdk.Detectors.Software.Ml
{
    sealed class NightlyMlSoftwareDetector : MlSoftwareDetector
    {
        public NightlyMlSoftwareDetector(IProductProvider productProvider, ISourceProvider sourceProvider)
            : base(productProvider, sourceProvider)
        {
        }

        protected override string String => "Magic Lantern ";

        protected override int StringCount => 6;

        protected override char SeparatorChar => '\n';

        protected override string GetVersionString(string[] strings)
        {
            return strings[0];
        }

        protected override string GetCreationDateString(string[] strings)
        {
            var builtStr = GetValue(strings, 1, "Built on ");
            if (builtStr == null)
                return null;
            var index = builtStr.IndexOf(" by ");
            if (index > 0)
                return builtStr.Substring(0, index);
            return builtStr;
        }

        protected override string GetCreator(string[] strings)
        {
            var builtStr = GetValue(strings, 1, "Built on ");
            if (builtStr == null)
                return null;
            var index = builtStr.IndexOf(" by ");
            if (index > 0)
                return builtStr.Substring(index + " by ".Length);
            return null;
        }

        protected override string GetPlatform(string[] strings)
        {
            return GetValue(strings, 1, "Camera");
        }

        protected override string GetRevision(string[] strings)
        {
            return GetValue(strings, 2, "Firmware");
        }

        protected override string GetStatus(string[] strings)
        {
            return string.Empty;
        }

        protected override string GetChangeset(string[] strings)
        {
            var value = GetValue(strings, 3, "Changeset")
                ?? GetValue(strings, 1, "Mercurial changeset");
            var split = value?.Split(' ');
            var split2 = split?[0].Split('+');
            return split2?[0];
        }

        private static string GetValue(string[] strings, int skip, string prefix)
        {
            return strings
                .Skip(skip)
                .FirstOrDefault(s => s.StartsWith(prefix))
                ?.TrimStart(prefix)
                ?.TrimStart(':', ' ');
        }
    }
}
