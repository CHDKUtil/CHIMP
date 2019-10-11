using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    interface ICategorySubstituteProvider
    {
        IDictionary<string, object>? GetSubstitutes(CameraInfo camera, CameraModelInfo cameraModel);
        IEnumerable<string> GetSupportedPlatforms();
    }
}
