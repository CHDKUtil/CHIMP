using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Card;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Camera
{
    public interface IInnerCameraDetector
    {
        CameraInfo GetCamera(CardInfo cardInfo, IProgress<double> progress, CancellationToken token);
    }
}
