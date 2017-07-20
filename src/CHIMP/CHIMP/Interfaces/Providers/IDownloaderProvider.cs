using Net.Chdk.Model.Software;

namespace Chimp
{
    interface IDownloaderProvider
    {
        IDownloader GetDownloader(string productName, string sourceName, SoftwareSourceInfo source);
    }
}
