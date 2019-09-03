namespace Net.Chdk.Meta.Model.CameraList
{
    public sealed class ListRevisionData : RevisionData<ListRevisionData, ListSourceData>
    {
        public string Status { get; set; }
        public bool Skip { get; set; }
    }
}
