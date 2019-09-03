using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Sdm
{
    sealed class SdmRevisionProvider : ProductRevisionProvider
    {
        public override string ProductName => "SDM";

        protected override RevisionData GetRevision(string revision, ListRevisionData listRevision, ListPlatformData list)
        {
            var key = listRevision.Source?.Revision ?? revision;
            if (!list.Revisions.ContainsKey(key))
                return null;

            return GetRevision(key);
        }
    }
}
