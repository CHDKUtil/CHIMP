namespace Net.Chdk.Model.Software
{
    public sealed class SoftwareSourceInfo
    {
        public string? Name { get; set; }
        public string? Channel { get; set; }
        public System.Uri? Url { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SoftwareSourceInfo source2
                && Name?.Equals(source2.Name) == true
                && Channel?.Equals(source2.Channel) == true
                && Url?.Equals(source2.Url) == true;
        }

        public override int GetHashCode()
        {
            var nameHashCode = Name != null ? Name.GetHashCode() : 0;
            var channelHashCode = Channel != null ? Channel.GetHashCode() : 0;
            var urlHashCode = Url != null ? Url.GetHashCode() : 0;
            return nameHashCode ^ channelHashCode ^ urlHashCode;
        }
    }
}
