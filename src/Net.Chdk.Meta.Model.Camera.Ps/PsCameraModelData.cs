using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera.Ps
{
    public sealed class PsCameraModelData : CameraModelData<PsCameraModelData, PsRevisionData>
    {
        public IDictionary<string, PsRevisionData> Revisions { get; set; }

        protected override IDictionary<string, PsRevisionData> GetRevisions() => Revisions;

        protected override void SetRevisions(IDictionary<string, PsRevisionData> value) => Revisions = value;
    }
}
