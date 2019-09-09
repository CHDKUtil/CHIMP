using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using System;

namespace Net.Chdk.Detectors.CameraModel
{
    static class CameraModelInfosExtensions
    {
        public static CameraModelInfo[] Collapse(this CameraModelInfo[] cameraModels, CameraInfo cameraInfo)
        {
            // IXUS 132/135
            if (cameraModels?.Length > 1 && cameraModels[0].Names.Length > 1)
            {
                for (int i = 0; i < cameraModels.Length; i++)
                {
                    var model = cameraModels[i];
                    foreach (var name in model.Names)
                    {
                        if (name.Equals(cameraInfo.Base.Model, StringComparison.OrdinalIgnoreCase))
                        {
                            return new[] { cameraModels[i] };
                        }
                    }
                }
            }
            return cameraModels;
        }
    }
}
