using System.Collections.Generic;

namespace Net.Chdk.Providers.Boot
{
    public interface IScriptProvider
    {
        uint GetBlockSize(string fileSystem);
        IDictionary<int, byte[]> GetBytes(string fileSystem);
        IDictionary<string, byte[]> GetFiles();
    }
}
