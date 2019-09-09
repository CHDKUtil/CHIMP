using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Camera
{
    abstract class ReverseCameraData
    {
        public string[] Models { get; set; }
        public uint ModelId { get; set; }
        public SoftwareEncodingInfo Encoding { get; set; }
        public AltInfo Alt { get; set; }
    }
}
