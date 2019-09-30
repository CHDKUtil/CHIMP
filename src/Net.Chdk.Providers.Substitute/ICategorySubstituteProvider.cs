using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    interface ICategorySubstituteProvider
    {
        IDictionary<string, string>? GetSubstitutes(CameraInfo camera, CameraModelInfo cameraModel);
    }
}
