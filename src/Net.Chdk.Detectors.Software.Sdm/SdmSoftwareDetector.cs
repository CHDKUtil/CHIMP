using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System;
using System.Globalization;

namespace Net.Chdk.Detectors.Software.Sdm
{
    sealed class SdmSoftwareDetector : SdmSoftwareDetectorBase
    {
        public SdmSoftwareDetector(IProductProvider productProvider, ISourceProvider sourceProvider)
            : base(productProvider, sourceProvider)
        {
        }

        protected override string String => "Writing info file...\0";
        protected override int StringCount => 14;

        protected override Version GetProductVersion(string[] strings)
        {
            return GetVersion(strings[3]);
        }

        protected override CultureInfo GetLanguage(string[] strings)
        {
            return GetCultureInfo("en");
        }

        protected override DateTime? GetCreationDate(string[] strings)
        {
            return GetCreationDate($"{strings[4]} {strings[5]}");
        }

        protected override string GetPlatform(string[] strings)
        {
            return strings[9];
        }

        protected override string GetRevision(string[] strings)
        {
            for (var i = 10; i < StringCount; i++)
                if (strings[i].Length > 0)
                    return strings[i];
            return null;
        }
    }
}
