using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Writers.Json;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera.Ps.Json
{
    sealed class JsonPsCameraWriter : JsonMetaWriter, IPsInnerCameraWriter
    {
        public void WriteCameras(string path, IDictionary<string, PsCameraData> cameras)
        {
            WriteJson(path, cameras);
        }
    }
}
