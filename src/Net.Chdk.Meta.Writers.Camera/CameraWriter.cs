using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Providers;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera
{
    public abstract class CameraWriter<TCamera, TModel, TCard> : SingleExtensionProvider<IInnerCameraWriter<TCamera, TModel, TCard>>
        where TCamera : CameraData<TCamera, TModel, TCard>
        where TModel : CameraModelData
        where TCard : CardData
    {
        protected CameraWriter(IEnumerable<IInnerCameraWriter<TCamera, TModel, TCard>> innerWriters)
            : base(innerWriters)
        {
        }

        public void WriteCameras(string path, IDictionary<string, TCamera> cameras)
        {
            var writer = GetInnerProvider(path, out string ext);
            if (writer == null)
                throw new InvalidOperationException($"Unknown camera writer extension: {ext}");
            writer.WriteCameras(path, cameras);
        }
    }
}
