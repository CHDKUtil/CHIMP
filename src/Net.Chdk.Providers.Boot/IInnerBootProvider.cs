using System.Collections.Generic;

namespace Net.Chdk.Providers.Boot
{
    interface IInnerBootProvider
    {
        string? FileName { get; }
        int[][]? Offsets { get; }
        byte[]? Prefix { get; }
        IDictionary<string, byte[]>? Files { get; }

        uint GetBlockSize(string fileSystem);
        IDictionary<int, byte[]>? GetBytes(string fileSystem);
    }
}
