using System;

namespace Net.Chdk.Providers.Software.Chdk
{
    public sealed class ChdkDownloadProvider : DownloadProvider
    {
        public ChdkDownloadProvider(Uri baseUri)
            : base(baseUri)
        {
        }
    }
}
