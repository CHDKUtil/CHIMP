using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    public interface ISubstituteProvider
    {
        IDictionary<string, string>? GetSubstitutes(CameraInfo camera, CameraModelInfo cameraModel);
    }
}
