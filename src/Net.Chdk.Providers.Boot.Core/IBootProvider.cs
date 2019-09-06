using System.Collections.Generic;

namespace Net.Chdk.Providers.Boot
{
    public interface IBootProvider
    {
        string GetFileName(string categoryName);
        int[][] GetOffsets(string categoryName);
        byte[] GetPrefix(string categoryName);
        uint GetBlockSize(string categoryName, string fileSystem);
        IDictionary<int, byte[]> GetBytes(string categoryName, string fileSystem);
    }
}
