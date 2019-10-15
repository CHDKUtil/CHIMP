using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Net.Chdk.Providers.Software
{
    public abstract class DownloadProvider : IDownloadProvider
    {
        private Uri BaseUri { get; }

        protected DownloadProvider(Uri baseUri)
        {
            BaseUri = baseUri;
        }

        public IEnumerable<IDownloadData>? GetDownloads(ISoftwareData software)
        {
            var matches = (software.Match as MatchData)?.Payload;
            if (matches == null)
                return null;
            return GetDownloads(matches, software.Info);
        }

        protected virtual IEnumerable<DownloadData> GetDownloads(Match[] matches, SoftwareInfo info)
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
