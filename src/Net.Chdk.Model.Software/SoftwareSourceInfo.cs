namespace Net.Chdk.Model.Software
{
    public sealed class SoftwareSourceInfo
    {
        public string Name { get; set; }
        public string Channel { get; set; }
        public System.Uri Url { get; set; }

        public override bool Equals(object obj)
        {
            var source2 = obj as SoftwareSourceInfo;
            if (source2 == null)
                return false;
            return Name.Equals(source2.Name) && Channel.Equals(source2.Channel) && Url.Equals(source2.Url);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Channel.GetHashCode() ^ Url.GetHashCode();
        }
    }
}
