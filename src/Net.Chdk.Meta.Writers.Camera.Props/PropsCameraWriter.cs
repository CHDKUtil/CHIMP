using Net.Chdk.Meta.Model.Camera;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Writers.Camera.Props
{
    sealed class PropsCameraWriter : IInnerCameraWriter
    {
        public void WriteCameras(string path, IDictionary<string, ICameraData> cameras)
        {
            using (var writer = File.CreateText(path))
            {
                WriteCameras(writer, cameras);
            }
        }

        public string Extension => ".properties";

        private void WriteCameras(TextWriter writer, IDictionary<string, ICameraData> cameras)
        {
            foreach (var kvp in cameras)
            {
                WriteModels(writer, kvp.Key, kvp.Value);
            }
        }

        private void WriteModels(TextWriter writer, string modelId, ICameraData camera)
        {
            var id = System.Convert.ToUInt32(modelId, 16);
            for (var i = 0u; i < camera.Models.Length; i++)
            {
                var model = camera.Models[i];
                WriteModel(writer, id + i, model);
            }
        }

        private void WriteModel(TextWriter writer, uint id, ICameraModelData model)
        {
            WriteRevisions(writer, id, model.Platform, model.Revisions);
        }

        private void WriteRevisions(TextWriter writer, uint id, string platform, IDictionary<string, IRevisionData> revisions)
        {
            foreach (var kvp in revisions)
            {
                WriteRevision(writer, id, platform, kvp.Key, kvp.Value.Revision);
            }
        }

        private void WriteRevision(TextWriter writer, uint id, string platform, string key, string value)
        {
            var revision = key.StartsWith("0x")
                ? GetRevision(key)
                : GetVersion(key);
            writer.WriteLine($"0x{id:x}-{revision}={platform}-{value}");
        }

        private static string GetRevision(string revisionKey)
        {
            var revision = System.Convert.ToUInt32(revisionKey, 16);
            return new string(new[] {
                    (char)(((revision >> 24) & 0x0f) + 0x30),
                    (char)(((revision >> 20) & 0x0f) + 0x30),
                    (char)(((revision >> 16) & 0x0f) + 0x30),
                    (char)(((revision >>  8) & 0x7f) + 0x60)
                });
        }

        private static string GetVersion(string versionKey)
        {
            var version = System.Version.Parse(versionKey);
            return $"{version.Major}{version.Minor}{version.Build}";
        }
    }
}
