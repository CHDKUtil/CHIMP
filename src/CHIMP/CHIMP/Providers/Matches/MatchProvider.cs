using Chimp.Model;
using Chimp.Properties;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly List<string> platforms;
        private readonly List<string> revisions;
        private readonly List<string> builds;

        protected MatchProvider(Uri baseUri, IDictionary<string, string> buildPaths, ILogger logger)
        {
            Logger = logger;
            BaseUri = baseUri;
            BuildPaths = buildPaths;

            platforms = new List<string>();
            revisions = new List<string>();
            builds = new List<string>();
        }

        public async Task<MatchData> GetMatchesAsync(SoftwareCameraInfo camera, string buildName, CancellationToken cancellationToken)
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

        private string GetError()
        {
            if (platforms.Count == 0)
                return nameof(Resources.Download_InvalidFormat_Text);

            if (revisions.Count == 0)
                return nameof(Resources.Download_UnsupportedModel_Text);

            if (builds.Count == 0)
                return nameof(Resources.Download_UnsupportedFirmware_Text);

            return nameof(Resources.Download_UnsupportedFirmware_Text);
        }

        private async Task<MatchData> GetMatchesAsync(SoftwareCameraInfo camera, string buildName, TextReader reader)
        {
            platforms.Clear();
            revisions.Clear();
            builds.Clear();

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var matches = GetMatches(camera, buildName, line);
                if (matches != null)
                    return matches;
            }

            var error = GetError();
            return new MatchData(error, platforms, revisions, builds);
        }

        protected abstract MatchData GetMatches(SoftwareCameraInfo camera, string buildName, string line);

        protected MatchData GetMatches(SoftwareCameraInfo camera, string buildName, Match match)
        {
            var platform = match.Groups["platform"].Value;
            if (camera.Platform.Equals(platform))
            {
                var matches = GetPlatformMatches(camera, buildName, match);
                if (matches != null)
                    return matches;
            }
            platforms.Add(platform);
            return null;
        }

        private MatchData GetPlatformMatches(SoftwareCameraInfo camera, string buildName, Match match)
        {
            var revision = match.Groups["revision"].Value;
            if (camera.Revision.Equals(revision))
            {
                var matches = GetRevisionMatches(camera, buildName, match);
                if (matches != null)
                    return matches;
            }
            revisions.Add(revision);
            return null;
        }

        private MatchData GetRevisionMatches(SoftwareCameraInfo _, string buildName, Match match)
        {
            var build = match.Groups["buildName"].Value;
            if (buildName.Equals(build))
            {
                var matches = GetMatches(buildName, match);
                if (matches != null)
                    return matches;
            }
            builds.Add(build);
            return null;
        }

        protected virtual MatchData GetMatches(string buildName, Match match)
        {
            return new MatchData(match);
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
    }
}
