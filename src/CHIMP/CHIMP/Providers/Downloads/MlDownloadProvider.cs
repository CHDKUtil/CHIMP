using System;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Downloads
{
    sealed class MlDownloadProvider : DownloadProvider
    {
        public MlDownloadProvider(Uri baseUri)
            : base(baseUri)
        {
        }

        protected override Uri GetBaseUri(Match match)
        {
            var url = match.Groups["url"].Value;
            return new Uri($"{url}artifact/");
        }
    }
}
