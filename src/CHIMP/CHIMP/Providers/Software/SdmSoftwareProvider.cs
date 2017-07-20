using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Software
{
    sealed class SdmSoftwareProvider : SoftwareProvider
    {
        public SdmSoftwareProvider(SoftwareSourceInfo source, CultureInfo language)
            : base(source, language)
        {
        }

        protected override string CategoryName => "PS";

        protected override string ProductName => "SDM";

        protected override Version GetVersion(Match match)
        {
            var version = match.Groups["version"].Value;
            return Version.Parse(version);
        }
    }
}
