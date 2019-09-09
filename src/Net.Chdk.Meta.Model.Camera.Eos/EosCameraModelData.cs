using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera.Eos
{
    public sealed class EosCameraModelData : CameraModelData<EosCameraModelData, EosRevisionData>
    {
        public IDictionary<string, EosRevisionData> Versions { get; set; }

        protected override IDictionary<string, EosRevisionData> GetRevisions() => Versions;

        protected override void SetRevisions(IDictionary<string, EosRevisionData> value) => Versions = value;
    }
}
