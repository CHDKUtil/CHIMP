using Newtonsoft.Json;

namespace Net.Chdk.Model.Software
{
    [JsonObject(IsReference = false)]
    public sealed class SoftwareCameraInfo
    {
        public string Platform { get; set; }
        public string Revision { get; set; }

        public override bool Equals(object obj)
        {
            var camera2 = obj as SoftwareCameraInfo;
            return Platform.Equals(camera2?.Platform) && Revision.Equals(camera2?.Revision);
        }

        public override int GetHashCode()
        {
            return Platform.GetHashCode() ^ Revision.GetHashCode();
        }

        public static bool operator ==(SoftwareCameraInfo camera1, SoftwareCameraInfo camera2)
        {
            if (ReferenceEquals(camera1, camera2))
                return true;
            if ((object)camera1 == null || (object)camera2 == null)
                return false;
            return camera1.Equals(camera2);
        }

        public static bool operator !=(SoftwareCameraInfo camera1, SoftwareCameraInfo camera2)
        {
            return !(camera1 == camera2);
        }
    }
}
