namespace Net.Chdk.Meta.Model.CameraTree
{
    public sealed class TreeRevisionData : RevisionData<TreeRevisionData, TreeSourceData>
    {
        public TreeIdData Id { get; set; }
        public TreeEncodingData Encoding { get; set; }
    }
}
