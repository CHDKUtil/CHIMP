using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkRevisionProvider : PsRevisionProvider
    {
        public override string ProductName => "CHDK";

        protected override RevisionData GetRevision(string revision, ListRevisionData listRevision, ListPlatformData list)
        {
            return GetRevision(revision);
        }
    }
}
