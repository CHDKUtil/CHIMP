using Chimp.Properties;
using Microsoft.Extensions.Logging;
using Net.Chdk.Adapters.Platform;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Providers.Matches
{
    abstract class MatchProvider : IMatchProvider
    {
        private ILogger Logger { get; }

        protected Uri BaseUri { get; }
        protected IDictionary<string, string> BuildPaths { get; }

        private IPlatformAdapter PlatformAdapter { get; }

        private bool hasMatch;
        private bool hasPlatform;
        private bool hasRevision;

        protected MatchProvider(Uri baseUri, IDictionary<string, string> buildPaths, IPlatformAdapter platformAdapter, ILogger logger)
        {
            Logger = logger;
            BaseUri = baseUri;
            BuildPaths = buildPaths;
            PlatformAdapter = platformAdapter;
        }

        public async Task<Match[]> GetMatchesAsync(SoftwareCameraInfo camera, string buildName, CancellationToken cancellationToken)
        {
            var buildUri = GetBuildUri(buildName);

            Logger.LogTrace("Fetching {0}", buildUri);

            using (var http = new HttpClient())
            using (var resp = await http.GetAsync(buildUri, cancellationToken))
            {
                try
                {
                    resp.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    Logger.LogError(0, ex, "Error fetching");
                    throw;
                }
                using (var stream = await resp.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    return await GetMatchesAsync(camera, buildName, reader);
                }
            }
        }

        public virtual string GetError()
        {
            if (!hasMatch)
                return nameof(Resources.Download_InvalidFormat_Text);

            if (!hasPlatform)
                return nameof(Resources.Download_UnsupportedModel_Text);

            if (!hasRevision)
                return nameof(Resources.Download_UnsupportedFirmware_Text);

            return nameof(Resources.Download_UnsupportedFirmware_Text);
        }

        private async Task<Match[]> GetMatchesAsync(SoftwareCameraInfo camera, string buildName, TextReader reader)
        {
            hasMatch = hasPlatform = hasRevision = false;

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var matches = GetMatches(camera, buildName, line);
                if (matches != null)
                    return matches.ToArray();
            }

            return null;
        }

        protected abstract IEnumerable<Match> GetMatches(SoftwareCameraInfo camera, string buildName, string line);

        protected abstract string ProductName { get; }

        protected IEnumerable<Match> GetMatches(SoftwareCameraInfo camera, string buildName, Match match)
        {
            hasMatch = true;
            var platform = NormalizePlatform(match.Groups["platform"].Value);
            if (camera.Platform.Equals(platform))
            {
                hasPlatform = true;
                if (camera.Revision.Equals(match.Groups["revision"].Value))
                {
                    hasRevision = true;
                    if (buildName.Equals(match.Groups["buildName"].Value))
                    {
                        return GetMatches(buildName, match);
                    }
                }
            }
            return null;
        }

        protected virtual IEnumerable<Match> GetMatches(string buildName, Match match)
        {
            return new[] { match };
        }

        private Uri GetBuildUri(string buildName)
        {
            string buildPath = null;
            BuildPaths?.TryGetValue(buildName, out buildPath);
            if (string.IsNullOrEmpty(buildPath))
                return BaseUri;
            var uriBuilder = new UriBuilder(BaseUri);
            uriBuilder.Path += buildPath;
            return uriBuilder.Uri;
        }

        private string NormalizePlatform(string platform)
        {
            return PlatformAdapter.NormalizePlatform(ProductName, platform);
        }
    }
}
