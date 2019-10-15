using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software
{
    public abstract class DownloadProvider : IDownloadProvider<MatchData, DownloadData>
    {
        private Uri BaseUri { get; }

        protected DownloadProvider(Uri baseUri)
        {
            BaseUri = baseUri;
        }

        public IEnumerable<DownloadData> GetDownloads(MatchData data, SoftwareInfo software)
        {
            var matches = data.Payload;
            if (matches == null)
                return Enumerable.Empty<DownloadData>();
            return GetDownloads(matches, software);
        }

        protected virtual IEnumerable<DownloadData> GetDownloads(Match[] matches, SoftwareInfo software)
        {
            yield return GetDownload(matches[0]);
        }

        protected DownloadData GetDownload(Match match)
        {
            return new DownloadData
            {
                BaseUri = GetBaseUri(match),
                Path = match.Groups["path"].Value,
                Date = match.Groups["date"].Value,
                Size = match.Groups["size"].Value,
            };
        }

        protected virtual Uri GetBaseUri(Match match)
        {
            return BaseUri;
        }
    }
}
