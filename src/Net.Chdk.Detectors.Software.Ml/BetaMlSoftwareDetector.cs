using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;

namespace Net.Chdk.Detectors.Software.Ml
{
    sealed class BetaMlSoftwareDetector : MlSoftwareDetector
    {
        public BetaMlSoftwareDetector(IProductProvider productProvider, ISourceProvider sourceProvider)
            : base(productProvider, sourceProvider)
        {
        }

        protected override string String => "alex@thinkpad\0";

        protected override int StringCount => 3;

        protected override string GetVersionString(string[] strings)
        {
            return strings[2];
        }

        protected override string GetCreationDateString(string[] strings)
        {
            return strings[0];
        }

        protected override string GetCreator(string[] strings)
        {
            return "alex@thinkpad";
        }

        protected override SoftwareCameraInfo GetCamera(string[] strings)
        {
            var split = strings[2].Split('.');
            var cameraStr = split[split.Length - 1];
            var startIndex = cameraStr.Length - 3;
            var platform = cameraStr.Substring(0, startIndex);
            var revision = cameraStr.Substring(startIndex);
            return GetCamera(platform, revision);
        }

        protected override string GetStatus(string[] strings)
        {
            return "beta";
        }

        protected override string GetChangeset(string[] strings)
        {
            return strings[1].Split(' ')[0];
        }
    }
}
