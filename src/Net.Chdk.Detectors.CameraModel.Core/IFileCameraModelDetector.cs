using System;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    public interface IFileCameraModelDetector
    {
        CameraModels GetCameraModels(string filePath, IProgress<double> progress, CancellationToken token);
    }
}
