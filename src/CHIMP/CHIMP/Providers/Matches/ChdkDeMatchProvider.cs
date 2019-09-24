using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Matches
{
    sealed class ChdkDeMatchProvider : MatchProvider
    {
        private static readonly Regex regex = new Regex("^(?<path>CHDK_DE_(?<platform>[0-9a-z_]+)-(?<revision>[0-9a-z]+)-(?<version>[0-9.]+)-(?<build>[0-9]+)(-(?<buildName>[a-z]+))?(_(?<status>[A-Z]+))?.zip)$");

        public ChdkDeMatchProvider(Uri baseUri, IDictionary<string, string> buildPaths, ILogger<ChdkDeMatchProvider> logger)
            : base(baseUri, buildPaths, logger)
        {
        }

        protected override IEnumerable<Match>? GetMatches(SoftwareCameraInfo camera, string buildName, string line)
        {
            var match = regex.Match(line);
            if (match.Success)
            {
                var result = GetMatches(camera, buildName, match);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
