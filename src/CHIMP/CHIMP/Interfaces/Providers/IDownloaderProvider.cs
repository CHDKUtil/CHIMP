using Net.Chdk.Model.Software;

namespace Chimp
{
    interface IDownloaderProvider
    {
        IDownloader GetDownloader(string productName, string sourceName = null, SoftwareSourceInfo source = null);
    }
}
