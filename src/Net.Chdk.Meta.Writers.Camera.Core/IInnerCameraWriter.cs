using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Providers;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera
{
    public interface IInnerCameraWriter<TCamera, TModel, TCard> : IExtensionProvider
        where TCamera : CameraData<TCamera, TModel, TCard>
        where TModel : CameraModelData
        where TCard : CardData
    {
        void WriteCameras(string path, IDictionary<string, TCamera> cameras);
    }
}
