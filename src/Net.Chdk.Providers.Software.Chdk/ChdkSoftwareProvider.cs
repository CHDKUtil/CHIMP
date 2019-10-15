using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software.Chdk
{
    public sealed class ChdkSoftwareProvider : SoftwareProvider
    {
        public ChdkSoftwareProvider(SoftwareSourceInfo source, CultureInfo language)
            : base(source, language)
        {
        }

        protected override string ProductName => "CHDK";

        protected override Version? GetVersion(Match? match)
        {
            if (match == null)
                return null;
            var version = match.Groups["version"].Value;
            var build = match.Groups["build"].Value;
            return Version.Parse($"{version}.{build}");
        }
    }
}
