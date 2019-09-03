using Net.Chdk.Meta.Model.Camera;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Writers.Camera.Props
{
    public abstract class PropsCameraWriter<TCamera, TModel, TCard>
        where TCamera : CameraData<TCamera, TModel, TCard>
        where TModel : CameraModelData
        where TCard : CardData
    {
        public void WriteCameras(string path, IDictionary<string, TCamera> cameras)
        {
            using (var writer = File.CreateText(path))
            {
                WriteCameras(writer, cameras);
            }
        }

        public string Extension => ".properties";

        private void WriteCameras(TextWriter writer, IDictionary<string, TCamera> cameras)
        {
            foreach (var kvp in cameras)
            {
                WriteModels(writer, kvp.Key, kvp.Value);
            }
        }

        private void WriteModels(TextWriter writer, string modelId, TCamera camera)
        {
            var id = System.Convert.ToUInt32(modelId, 16);
            for (var i = 0u; i < camera.Models.Length; i++)
            {
                var model = camera.Models[i];
                WriteModel(writer, id + i, model);
            }
        }

        protected abstract void WriteModel(TextWriter writer, uint id, TModel model);
    }
}
