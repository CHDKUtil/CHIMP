using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera.Eos
{
    public sealed class EosCameraModelData : CameraModelData<VersionData>
    {
        public IDictionary<string, VersionData> Versions { get; set; }

        protected override IDictionary<string, VersionData> GetRevisions() => Versions;
    }
}
