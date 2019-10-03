using Net.Chdk.Meta.Providers.Platform;
using Net.Chdk.Meta.Writers.Json;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Platform.Json
{
    sealed class JsonPlatformWriter : JsonMetaWriter, IPlatformWriter
    {
        public void WritePlatforms(string path, IDictionary<string, CameraModel[]> platforms)
        {
            WriteJson(path, platforms);
        }
    }
}
