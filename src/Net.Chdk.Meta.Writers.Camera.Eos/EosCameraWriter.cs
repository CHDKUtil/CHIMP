using Net.Chdk.Meta.Model.Camera.Eos;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Camera.Eos
{
    sealed class EosCameraWriter : CameraWriter<EosCameraData, EosCameraModelData, EosCardData>, IEosCameraWriter
    {
        public EosCameraWriter(IEnumerable<IEosInnerCameraWriter> innerWriters)
            : base(innerWriters)
        {
        }
    }
}
