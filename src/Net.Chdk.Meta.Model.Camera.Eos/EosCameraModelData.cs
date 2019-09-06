using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera.Eos
{
    public sealed class EosCameraModelData : CameraModelData<EosCameraModelData, VersionData>
    {
        public IDictionary<string, VersionData> Versions { get; set; }

        protected override IDictionary<string, VersionData> GetRevisions() => Versions;

        protected override void SetRevisions(IDictionary<string, VersionData> value) => Versions = value;
    }
}
