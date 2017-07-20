using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Matches
{
    sealed class MlMatchProvider : MatchProvider
    {
        private static readonly Regex regex = new Regex("\"artifacts\":\\[{\"relativePath\":\"(?<path>platform/(?<platform>[0-9A-Z]+).(?<revision>[0-9]+)/magiclantern-(?<prefix>Nightly).(?<date>[0-9A-Za-z]+).[0-9A-Z]+.zip)\"}],\"url\":\"(?<url>[^\"]+)\"");

        public MlMatchProvider(Uri baseUri, IDictionary<string, string> buildPaths, ILogger<MlMatchProvider> logger)
            : base(baseUri, buildPaths, logger)
        {
        }

        protected override IEnumerable<Match> GetMatches(SoftwareCameraInfo camera, string buildName, string line)
        {
            var matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                var result = GetMatches(camera, buildName, match);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
