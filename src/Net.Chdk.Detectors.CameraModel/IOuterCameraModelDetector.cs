using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    public interface IOuterCameraModelDetector
    {
        (CameraInfo, CameraModelInfo[]?)? GetCameraModels(CardInfo cardInfo, SoftwareInfo? softwareInfo, IProgress<double>? progress, CancellationToken token);
    }
}
