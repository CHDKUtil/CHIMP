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
                && Name == source2.Name
                && Channel == source2.Channel
                && Url == source2.Url;
        }

        public override int GetHashCode()
        {
            var nameHashCode = Name != null ? Name.GetHashCode() : 0;
            var channelHashCode = Channel != null ? Channel.GetHashCode() : 0;
            var urlHashCode = Url != null ? Url.GetHashCode() : 0;
            return nameHashCode ^ channelHashCode ^ urlHashCode;
        }

        public static bool operator ==(SoftwareSourceInfo? source1, SoftwareSourceInfo? source2)
        {
            if (ReferenceEquals(source1, source2))
                return true;
            if (source1 is null || source2 is null)
                return false;
            return source1.Equals(source2);
        }

        public static bool operator !=(SoftwareSourceInfo? source1, SoftwareSourceInfo? source2)
        {
            return !(source1 == source2);
        }
    }
}
