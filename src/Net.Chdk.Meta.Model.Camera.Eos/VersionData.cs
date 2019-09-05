namespace Net.Chdk.Meta.Model.Camera.Eos
{
    public sealed class VersionData : IRevisionData
    {
        public string Version { get; set; }

        string IRevisionData.Revision => Version;
    }
}
