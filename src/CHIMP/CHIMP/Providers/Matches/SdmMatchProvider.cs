using Chimp.Model;
using Chimp.Properties;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Matches
{
    sealed class SdmMatchProvider : MatchProvider
    {
        private static readonly Regex commonRegex = new Regex("(?<path>Common_files.zip)");
        private static readonly Regex regex = new Regex("(?<path>SDM-CA-(?<platform>[0-9a-z]+)-(?<revision>[0-9a-z]+)-(?<version>[0-9.]+).zip)");

        private Match commonMatch;

        public SdmMatchProvider(Uri baseUri, IDictionary<string, string> buildPaths, ILogger<SdmMatchProvider> logger)
            : base(baseUri, buildPaths, logger)
        {
        }

        protected override MatchData GetMatches(SoftwareCameraInfo camera, string buildName, string line)
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

        protected override MatchData GetMatches(string buildName, Match match)
        {
            if (commonMatch == null)
                return new MatchData(nameof(Resources.Download_InvalidFormat_Text));
            return new MatchData(commonMatch, match);
        }
    }
}
