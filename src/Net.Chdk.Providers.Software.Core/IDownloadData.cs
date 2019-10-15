namespace Net.Chdk.Providers.Software
{
    public interface IDownloadData
    {
        string? TargetPath { get; }
        string? Path { get; }
        string? RootDir { get; }
    }
}
