using Net.Chdk.Meta.Model.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;

namespace Net.Chdk.Providers.Platform
{
    public interface IPlatformProvider
    {
        string? GetPlatform(CameraInfo camera, CameraModelInfo cameraModel, string categoryName);
        PlatformData? GetPlatform(string platform, string categoryName);
        PlatformData[]? GetPlatforms(CameraInfo camera, string categoryName);
    }
}
