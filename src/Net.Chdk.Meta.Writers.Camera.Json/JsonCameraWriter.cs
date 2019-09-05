using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Writers.Json;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera
{
    sealed class JsonCameraWriter : JsonMetaWriter, IInnerCameraWriter
    {
        public void WriteCameras(string path, IDictionary<string, ICameraData> cameras)
        {
            WriteJson(path, cameras);
        }
    }
}
