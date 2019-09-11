using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Providers;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera
{
    public interface IInnerCameraWriter : IExtensionProvider
    {
        void WriteCameras(string path, IDictionary<string, CameraData> cameras);
    }
}
