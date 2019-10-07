using Newtonsoft.Json;

namespace Net.Chdk.Model.Software
{
    [JsonObject(IsReference = false)]
    public sealed class SoftwareCameraInfo
    {
        public string? Platform { get; set; }
        public string? Revision { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SoftwareCameraInfo camera2
                && Platform?.Equals(camera2.Platform) == true
                && Revision?.Equals(camera2.Revision) == true;
        }

        public override int GetHashCode()
        {
            var platformHashCode = Platform != null ? Platform.GetHashCode() : 0;
            var revisionHashCode = Revision != null ? Revision.GetHashCode() : 0;
            return platformHashCode ^ revisionHashCode;
        }

        public static bool operator ==(SoftwareCameraInfo? camera1, SoftwareCameraInfo? camera2)
        {
            if (ReferenceEquals(camera1, camera2))
                return true;
            if (camera1 is null || camera2 is null)
                return false;
            return camera1.Equals(camera2);
        }

        public static bool operator !=(SoftwareCameraInfo? camera1, SoftwareCameraInfo? camera2)
        {
            return !(camera1 == camera2);
        }
    }
}
