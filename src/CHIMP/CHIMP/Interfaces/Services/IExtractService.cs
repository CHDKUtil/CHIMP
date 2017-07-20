using System.Threading;

namespace Chimp
{
    interface IExtractService
    {
        string Extract(string path, string filePath, string dirPath, string tempPath, CancellationToken cancellationToken);
    }
}
