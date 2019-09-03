using Net.Chdk.Meta.Model.Camera.Ps;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera.Ps
{
    sealed class PsCameraWriter : CameraWriter<PsCameraData, PsCameraModelData, PsCardData>, IPsCameraWriter
    {
        public PsCameraWriter(IEnumerable<IPsInnerCameraWriter> innerWriters)
            : base(innerWriters)
        {
        }
    }
}
