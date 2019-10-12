using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Software
{
    sealed class MlSoftwareProvider : SoftwareProvider
    {
        public MlSoftwareProvider(SoftwareSourceInfo source, CultureInfo language)
            : base(source, language)
        {
        }

        protected override string ProductName => "ML";

        protected override Version GetVersion(Match match)
        {
            var date = GetCreationDate(match);
            if (date == null)
                return null;
            return new Version(date.Value.Year, date.Value.Month, date.Value.Day);
        }

        protected override string GetVersionPrefix(Match match)
        {
            return match?.Groups["prefix"].Value;
        }
    }
}
