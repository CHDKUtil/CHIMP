using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Matches
{
    sealed class SdmMatchProvider : MatchProvider
    {
        private static readonly Regex commonRegex = new Regex("<a href=\"(?<path>Common_files.zip)\">Common_files.zip</a> +(?<date>[0-9]{1,2}-[A-Z][a-z]{2}-[1-9][0-9]{3} [0-9]{2}:[0-9]{2}) +(?<size>[0-9]+K)");
        private static readonly Regex regex = new Regex("<a href=\"(?<path>SDM-CA-(?<platform>[0-9a-z]+)-(?<revision>[0-9a-z]+)-(?<version>[0-9.]+).zip)\">.+</a> +(?<date>[0-9]{1,2}-[A-Z][a-z]{2}-[1-9][0-9]{3} [0-9]{2}:[0-9]{2}) +(?<size>[0-9]+K)");

        private Match commonMatch;

        public SdmMatchProvider(Uri baseUri, IDictionary<string, string> buildPaths, ILogger<SdmMatchProvider> logger)
            : base(baseUri, buildPaths, logger)
        {
        }

        protected override IEnumerable<Match> GetMatches(SoftwareCameraInfo camera, string buildName, string line)
        {
            var match = commonRegex.Match(line);
            if (match.Success)
            {
                commonMatch = match;
                return null;
            }

            match = regex.Match(line);
            if (match.Success)
            {
                var result = GetMatches(camera, buildName, match);
                if (result != null)
                    return result;
            }

            return null;
        }

        protected override IEnumerable<Match> GetMatches(string buildName, Match match)
        {
            if (commonMatch != null)
                yield return commonMatch;
            yield return match;
        }
    }
}
