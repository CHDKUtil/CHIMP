using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Providers;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera
{
    sealed class CameraWriter : SingleExtensionProvider<IInnerCameraWriter>, ICameraWriter
    {
        public CameraWriter(IEnumerable<IInnerCameraWriter> innerWriters)
            : base(innerWriters)
        {
        }

        public void WriteCameras(string path, IDictionary<string, CameraData> cameras)
        {
            var writer = GetInnerProvider(path, out string ext);
            if (writer == null)
                throw new InvalidOperationException($"Unknown camera writer extension: {ext}");
            writer.WriteCameras(path, cameras);
        }
    }
}
