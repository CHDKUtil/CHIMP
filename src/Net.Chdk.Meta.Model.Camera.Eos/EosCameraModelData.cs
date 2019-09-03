using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera.Eos
{
    public sealed class EosCameraModelData : CameraModelData
    {
        public IDictionary<string, VersionData> Versions { get; set; }
    }
}
