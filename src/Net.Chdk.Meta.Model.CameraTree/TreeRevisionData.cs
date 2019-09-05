namespace Net.Chdk.Meta.Model.CameraTree
{
    public sealed class TreeRevisionData : RevisionData<TreeRevisionData, TreeSourceData>
    {
        public ushort? Id { get; set; }
        public byte? Encoding { get; set; }
    }
}
