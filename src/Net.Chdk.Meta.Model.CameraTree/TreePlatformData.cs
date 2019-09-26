namespace Net.Chdk.Meta.Model.CameraTree
{
    public sealed class TreePlatformData : PlatformData<TreePlatformData, TreeRevisionData, PlatformSourceData>
    {
        public byte? Encoding { get; set; }
        public bool MultiCard { get; set; }
        public string[]? AltNames { get; set; }
    }
}
