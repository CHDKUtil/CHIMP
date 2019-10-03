using System.Collections.Generic;
using Net.Chdk.Meta.Providers.Platform;

namespace Net.Chdk.Meta.Writers.Platform
{
    public interface IPlatformWriter
    {
        void WritePlatforms(string path, IDictionary<string, CameraModel[]> platforms);
    }
}
