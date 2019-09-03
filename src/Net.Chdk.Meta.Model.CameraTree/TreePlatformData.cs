namespace Net.Chdk.Meta.Model.CameraTree
{
    public sealed class TreePlatformData : PlatformData<TreePlatformData, TreeRevisionData, TreeSourceData>
    {
        public TreeIdData Id { get; set; }
        public TreeEncodingData Encoding { get; set; }
        public TreeCardData Card { get; set; }
        public TreeAltData Alt { get; set; }
    }
}
