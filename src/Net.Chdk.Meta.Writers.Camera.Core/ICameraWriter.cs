using Net.Chdk.Meta.Model.Camera;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera
{
    public interface ICameraWriter
    {
        void WriteCameras(string path, IDictionary<string, CameraData> cameras);
    }
}
