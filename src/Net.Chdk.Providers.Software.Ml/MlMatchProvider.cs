using Microsoft.Extensions.Logging;
using Net.Chdk.Adapters.Platform;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software.Ml
{
    public sealed class MlMatchProvider : MatchProvider
    {
        private static readonly Regex regex = new Regex("\"artifacts\":\\[{\"relativePath\":\"(?<path>platform/(?<platform>[0-9A-Z]+).(?<revision>[0-9]+)/magiclantern-(?<prefix>Nightly).(?<date>[0-9A-Za-z]+).[0-9A-Z]+.zip)\"}],\"url\":\"(?<url>[^\"]+)\"");

        public MlMatchProvider(Uri baseUri, IDictionary<string, string> buildPaths, IPlatformAdapter platformAdapter, ILogger<MlMatchProvider> logger)
            : base(baseUri, buildPaths, platformAdapter, logger)
        {
        }

        protected override MatchData? GetMatches(SoftwareCameraInfo camera, string buildName, string line)
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

        protected override string ProductName => "ML";
    }
}
