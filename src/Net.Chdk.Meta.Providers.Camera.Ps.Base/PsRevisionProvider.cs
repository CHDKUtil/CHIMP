using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public abstract class PsRevisionProvider : ProductRevisionProvider
    {
        protected override string GetRevisionKey(string revisionStr)
        {
            var revision = GetFirmwareRevision(revisionStr);
            return $"0x{revision:x}";
        }

        protected override IRevisionData GetRevision(string revision)
        {
            return new RevisionData
            {
                Revision = revision,
            };
        }

        private static uint GetFirmwareRevision(string revision)
        {
            return
                (uint)((revision[0] - 0x30) << 24) +
                (uint)((revision[1] - 0x30) << 20) +
                (uint)((revision[2] - 0x30) << 16) +
                (uint)((revision[3] - 0x60) << 8);
        }
    }
}
