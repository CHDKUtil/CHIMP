using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera.Ps
{
    public sealed class PsCameraModelData : CameraModelData<RevisionData>
    {
        public IDictionary<string, RevisionData> Revisions { get; set; }

        protected override IDictionary<string, RevisionData> GetRevisions() => Revisions;
    }
}
