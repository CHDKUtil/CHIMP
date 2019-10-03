using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Platform
{
    public interface IPlatformProvider
    {
        IDictionary<string, CameraModel[]> GetPlatforms(string path, string? category = null);
    }
}
