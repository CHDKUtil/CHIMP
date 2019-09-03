using Net.Chdk.Meta.Model.Camera;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera
{
    public interface ICameraWriter<TCamera, TModel, TCard>
        where TCamera : CameraData<TCamera, TModel, TCard>
        where TModel : CameraModelData
        where TCard : CardData
    {
        void WriteCameras(string path, IDictionary<string, TCamera> cameras);
    }
}
