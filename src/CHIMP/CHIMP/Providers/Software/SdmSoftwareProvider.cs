using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Software
{
    sealed class SdmSoftwareProvider : SoftwareProvider
    {
        public SdmSoftwareProvider(IProductProvider productProvider, SoftwareSourceInfo source, CultureInfo language)
            : base(productProvider, source, language)
        {
        }

        protected override string ProductName => "SDM";

        protected override Version GetVersion(Match match)
        {
            if (match == null)
                return null;
            var version = match.Groups["version"].Value;
            return Version.Parse(version);
        }
    }
}
