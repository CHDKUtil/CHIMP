namespace Net.Chdk.Meta.Model.Camera.Eos
{
    public sealed class EosRevisionData : IRevisionData
    {
        public string Version { get; set; }

        string IRevisionData.Revision
        {
            get => Version;
            set { Version = value; }
        }
    }
}
