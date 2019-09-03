using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera.Ps
{
    public sealed class PsCameraModelData : CameraModelData
    {
        public IDictionary<string, RevisionData> Revisions { get; set; }
    }
}
