using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    public interface IFileCameraModelDetector
    {
        (CameraInfo, CameraModelInfo[]?)? GetCameraModels(string filePath, IProgress<double>? progress, CancellationToken token);
    }
}
