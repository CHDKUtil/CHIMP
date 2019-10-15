using Chimp.Model;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Downloads
{
    abstract class DownloadProvider : IDownloadProvider
    {
        private Uri BaseUri { get; }

        protected DownloadProvider(Uri baseUri)
        {
            BaseUri = baseUri;
        }

        public IEnumerable<DownloadData> GetDownloads(SoftwareData software)
        {
            if (!(software.Match is MatchData matchData))
                return null;
            return GetDownloads(matchData.Matches, software.Info);
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
