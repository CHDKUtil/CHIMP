using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.CameraModel
{
    public interface ICameraModelProvider
    {
        SoftwareEncodingInfo? GetEncoding(SoftwareProductInfo product, SoftwareCameraInfo camera);
        string? GetAltButton(SoftwareProductInfo product, SoftwareCameraInfo camera);
        string? GetCardType(SoftwareProductInfo product, CameraInfo camera);
        string? GetCardSubtype(SoftwareProductInfo product, CameraInfo camera);
        string? GetBootFileSystem(SoftwareProductInfo product, CameraInfo camera);
        bool? IsMultiPartition(SoftwareProductInfo product, CameraInfo camera);
    }
}
