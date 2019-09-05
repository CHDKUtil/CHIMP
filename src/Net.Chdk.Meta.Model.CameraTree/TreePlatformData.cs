namespace Net.Chdk.Meta.Model.CameraTree
{
    public sealed class TreePlatformData : PlatformData<TreePlatformData, TreeRevisionData, TreeSourceData>
    {
        public ushort? Id { get; set; }
        public byte? Encoding { get; set; }
        public bool MultiCard { get; set; }
        public TreeAltData Alt { get; set; }
    }
}
