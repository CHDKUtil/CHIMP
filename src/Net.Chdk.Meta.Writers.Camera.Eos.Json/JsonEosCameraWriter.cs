using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Meta.Writers.Json;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera.Eos.Json
{
    sealed class JsonEosCameraWriter : JsonMetaWriter, IEosInnerCameraWriter
    {
        public void WriteCameras(string path, IDictionary<string, EosCameraData> cameras)
        {
            WriteJson(path, cameras);
        }
    }
}
