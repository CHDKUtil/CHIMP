using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software.Sdm
{
    public sealed class SdmSoftwareProvider : SoftwareProvider
    {
        public SdmSoftwareProvider(SoftwareSourceInfo source, CultureInfo language)
            : base(source, language)
        {
        }

        protected override string ProductName => "SDM";

        protected override Version? GetVersion(Match? match)
        {
            if (match == null)
                return null;
            var version = match.Groups["version"].Value;
            return Version.Parse(version);
        }
    }
}
